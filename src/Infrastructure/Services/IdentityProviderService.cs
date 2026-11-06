using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.Infrastructure.Services;

public class IdentityProviderService(HttpClient httpClient, IConfiguration configuration, IUserRepository userRepository, ILogger<IdentityProviderService> logger) {
	private string? adminToken;
	private DateTime tokenExpiry = DateTime.MinValue;

	private async Task<Result<T>> ExecuteHttpRequestAsync<T>(Func<Task<T>> httpCall, Error error) {
		try { return Result.Success(await httpCall()); }
		catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) {
			logger.LogError(ex, "Error occurred while executing HTTP request");
			return Result.Failure<T>(error);
		}
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
			Errors.AdminTokenFailed(HttpStatusCode.ServiceUnavailable)
		);

		if (result.IsFailure) return Result.Failure<string>(result.Errors!);

		var response = result.Data!;
		if (!response.IsSuccessStatusCode) {
			var errorContent = await response.Content.ReadAsStringAsync();
			logger.LogError($"Keycloak user creation failed. Status: {response.StatusCode}");
			return Result.Failure<string>(Errors.AdminTokenFailed(response.StatusCode));
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
				$"{configuration["Auth:AdminUrl"]}partialImport",
				JsonContent.Create(new {
					ifResourceExists = "FAIL",
					users = new[] {
						new {
							id, username, email,
							credentials = new[] { new { type = "password", value = password, temporary = false } },
							createdTimestamp = createdAt.ToUnixTimeMilliseconds(),
							enabled = true
						}
					}
				})
			),
			Errors.CreateUserFailed
		);

		if (result.IsFailure) return Result.Failure<Unit>(result.Errors!);

		var response = result.Data!;
		if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
			return Result.Failure<Unit>(Errors.UserExists);

		if (!response.IsSuccessStatusCode) {
			var errorContent = await response.Content.ReadAsStringAsync();
			return Result.Failure<Unit>(Errors.CreateUserFailed);
		}

		var sendEmailResult = await ExecuteHttpRequestAsync(
			async () => await httpClient.PutAsync(
				$"{configuration["Auth:AdminUrl"]}users/{id}/send-verify-email",
				null
			),
			Errors.AuthUnavailable
		);

		return Result.Success(Unit.Value);
	}

	static class Errors {
		public static readonly Error AuthUnavailable = new("Keycloak.Unavailable", $"Failed to connect to Keycloak");
		public static readonly Error UserExists = new("Keycloak.UserExists", "User already exists in Keycloak");
		public static readonly Error CreateUserFailed = new("Keycloak.CreateFailed", "Failed to create user in Keycloak");
		public static Error AdminTokenFailed(HttpStatusCode status) => new("Keycloak.AdminAuthFailed", $"Failed to get admin token. Status: {status}");
	}
}