using WomensWiki.Common.Interfaces;
using WomensWiki.Domain;

namespace WomensWiki.Features.Users.Persistence;

public interface IUserRepository : IRepository {
	Task<User?> GetUserByUsername(string username);
	Task<User> CreateUser(string username, string email, string passwordHash);
	Task SetRefreshToken(User user, string token, DateTime expiry);
}