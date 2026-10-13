using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.Infrastructure.Services;

public class IdentityProviderService(HttpClient httpClient, IConfiguration configuration, IUserRepository userRepository) {
	private string? adminToken;
	private DateTime tokenExpiry = DateTime.MinValue;

	private static async Task<Result<T>> ExecuteHttpRequestAsync<T>(Func<Task<T>> httpCall, string errorCode, string errorMessage) {
		try { return Result.Success(await httpCall()); }
		catch (HttpRequestException ex) { return Result.Failure<T>([new Error(errorCode, $"{errorMessage}: {ex.Message}")]); }
		catch (TaskCanceledException) { return Result.Failure<T>([new Error(errorCode, $"{errorMessage}: Request timeout")]); }
	}

	public async Task TransformClaimsAsync(TokenValidatedContext context) {
		var identity = context.Principal?.Identity as ClaimsIdentity;
		if (identity == null ||
			!Guid.TryParse(identity.FindFirst("sub")?.Value, out var userGuid)
		){
			context.Fail("Token has no claims identity or an invalid subject");
			return;
		}

		var user = await userRepository.GetUserById(userGuid);
		foreach (var role in user?.Roles ?? []) {
			if (Enum.TryParse<Common.Constants.Role>(role.Name, ignoreCase: true, out var roleEnum)) {
				identity.AddClaim(new Claim(ClaimTypes.Role, roleEnum.ToString()));
			}
		}
	}

	private async Task<Result<string>> GetAdminTokenAsync() {
		if (adminToken != null && DateTime.UtcNow < tokenExpiry) return Result.Success(adminToken);

		var result = await ExecuteHttpRequestAsync(
			async () => await httpClient.PostAsync(
				"protocol/openid-connect/token",
				new FormUrlEncodedContent(new Dictionary<string, string> {
					["grant_type"] = "client_credentials",
					["client_id"] = configuration["Auth:ClientId"]!,
					["client_secret"] = configuration["Auth:ClientSecret"]!
				})
			),
			"Keycloak.Unavailable", "Failed to connect to Keycloak"
		);

		if (result.IsFailure) return Result.Failure<string>(result.Errors!);

		var response = result.Data!;
		if (!response.IsSuccessStatusCode) {
			var errorContent = await response.Content.ReadAsStringAsync();
			return Result.Failure<string>([new Error("Keycloak.AuthFailed", $"Failed to get admin token. Status: {response.StatusCode}, Error: {errorContent}")]);
		}
		
		var tokenResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
		adminToken = tokenResponse.GetProperty("access_token").GetString();
		tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.GetProperty("expires_in").GetInt32() - 60);
		
		return Result.Success(adminToken!);
	}

	public async Task<Result<bool>> RemoteUserExistsAsync(string username, string email) {
		var tokenResult = await GetAdminTokenAsync();
		if (tokenResult.IsFailure) return Result.Failure<bool>(tokenResult.Errors!);

		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Data!);

		var usernameResponse = await httpClient.GetAsync($"users?username={Uri.EscapeDataString(username)}&exact=true");
		if (usernameResponse.IsSuccessStatusCode) {
			var usernameUsers = await usernameResponse.Content.ReadFromJsonAsync<JsonElement[]>();
			if (usernameUsers?.Length > 0) return Result.Success(true);
		}

		var emailResponse = await httpClient.GetAsync($"users?email={Uri.EscapeDataString(email)}&exact=true");
		if (emailResponse.IsSuccessStatusCode) {
			var emailUsers = await emailResponse.Content.ReadFromJsonAsync<JsonElement[]>();
			if (emailUsers?.Length > 0) return Result.Success(true);
		}

		return Result.Success(false);
	}

	public async Task<Result<Unit>> CreateRemoteUserAsync(Guid id, DateTimeOffset createdAt, string username, string email, string password) {
		var tokenResult = await GetAdminTokenAsync();
		if (tokenResult.IsFailure) return Result.Failure<Unit>(tokenResult.Errors!);

		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Data!);

		var result = await ExecuteHttpRequestAsync(
			async () => await httpClient.PostAsync(
				$"{configuration["Auth:AdminUrl"]}users",
				JsonContent.Create(new {
					id, username, email,
					enabled = true,
					credentials = new[] { new { type = "password", value = password, temporary = false } }
				})
			),
			"Keycloak.Unavailable", "Failed to connect to Keycloak"
		);

		if (result.IsFailure) return Result.Failure<Unit>(result.Errors!);

		var response = result.Data!;
		if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
			return Result.Failure<Unit>([new Error("Keycloak.UserExists", "User already exists in Keycloak.")]);

		if (!response.IsSuccessStatusCode) {
			var errorContent = await response.Content.ReadAsStringAsync();
			return Result.Failure<Unit>([new Error("Keycloak.CreateFailed", $"Failed to create user in Keycloak: {errorContent}")]);
		}

		var sendEmailResult = await ExecuteHttpRequestAsync(
			async () => await httpClient.PutAsync(
				$"{configuration["Auth:AdminUrl"]}users/{id}/send-verify-email",
				null
			),
			"Keycloak.Unavailable", "Failed to connect to Keycloak"
		);

		return Result.Success(Unit.Value);
	}
}