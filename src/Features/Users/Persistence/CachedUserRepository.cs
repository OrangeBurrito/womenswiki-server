using Microsoft.Extensions.Caching.Memory;
using WomensWiki.Domain;
using WomensWiki.Domain.Users;
using WomensWiki.Common;

namespace WomensWiki.Features.Users.Persistence;

public class CachedUserRepository(UserRepository repository, IMemoryCache cache) : IUserRepository {
	public Task<User> CreateUser(string username, string email) 
		=> repository.CreateUser(username, email);

	public Task<User?> GetUserById(Guid id) 
		=> repository.GetUserById(id);

	public Task<User?> GetUserByUsername(string username) 
		=> repository.GetUserByUsername(username);

	public Task<User?> GetUserByEmail(string email) 
		=> repository.GetUserByEmail(email);

	public async Task DeleteNewUser(User user) {
		cache.Remove($"user_{user.Id}");
		await repository.DeleteNewUser(user);
	}

	public async Task<User?> UpdateUserRole(User user, User adminUser, Role role) {
		cache.Remove($"user_role_{user.Id}");
		return await repository.UpdateUserRole(user, adminUser, role);
	}

	public async Task<Constants.Role?> GetUserRole(User user) {
		return await cache.GetOrCreateAsync($"user_role_{user.Id}", async entry => {
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
			return await repository.GetUserRole(user);
		});
	}
}
