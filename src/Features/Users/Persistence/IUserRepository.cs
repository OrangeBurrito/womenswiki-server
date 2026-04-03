using WomensWiki.Infrastructure.Interfaces;
using WomensWiki.Domain.Users;
using WomensWiki.Domain;
using WomensWiki.Common;

namespace WomensWiki.Features.Users.Persistence;

public interface IUserRepository : IRepository {
	Task<User?> GetUserById(Guid id);
	Task<User?> GetUserByUsername(string username);
	Task<User?> GetUserByEmail(string email);
	Task<User> CreateUser(string username, string email);
	Task<User?> UpdateUserRole(User user, User adminUser, Role role);
	Task<Constants.Role?> GetUserRole(User user);
	Task DeleteNewUser(User user);
}