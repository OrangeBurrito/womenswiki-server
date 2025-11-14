using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WomensWiki.src.Common;

public class AuthService(IConfiguration configuration) {

	public string HashPassword(string password) {
		return "";
		// return Argon2.Hash(password);
	}

	public string GenerateJwtToken(Guid userId, string username) {
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:SigningSecret"]));

		var token = new JwtSecurityToken(
			issuer: configuration["Auth:ClientId"],
			audience: configuration["Auth:Audience"],
			claims: [
				new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			],
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken() {
		var rand = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(rand);
		return Convert.ToBase64String(rand);
	}
}