using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace WomensWiki.Common.Middleware;

public class APIKeyMiddleware(RequestDelegate next, IConfiguration configuration) {
	private readonly byte[] apiKey = Encoding.UTF8.GetBytes(configuration["Auth:ApiKey"] ?? throw new InvalidOperationException("ApiKey is not configured"));

	public async Task InvokeAsync(HttpContext context) {
		if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedKey)) {
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("API Key was not provided");
			return;
		}

		if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(extractedKey), apiKey)) {
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("Unauthorized client");
			return;
		}

		var forwardedIp = context.Request.Headers["X-Forwarded-For"].ToString().Split(",")[0].Trim(); // comma delimited formatting
		if (IPAddress.TryParse(forwardedIp, out var clientIp)) {
			context.Connection.RemoteIpAddress = clientIp;
		}

		await next(context);
	}
}