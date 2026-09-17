using Microsoft.EntityFrameworkCore;
using WomensWiki.Infrastructure.Persistence;
using WomensWiki.Domain;
using WomensWiki.Domain.Users;
using WomensWiki.Common;

namespace WomensWiki.Features.Users.Persistence;

public class UserRepository(AppDbContext dbContext) : IUserRepository {
    public async Task<User> CreateUser(string username, string email) {
        var userRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == Constants.Role.User.ToString().ToLowerInvariant());
        
        var user = User.Create(username, email);
		user.AddRole(userRole!);
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

	public Task<User?> GetUserById(Guid id) {
		return dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
    }

	public async Task<User?> GetUserByUsername(string username) {
		return await dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
	}

	public async Task<User?> GetUserByEmail(string email) {
		return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
	}

	public async Task<User?> UpdateUserRole(User user, User adminUser, Role role) {
		var adminRole = dbContext.Roles.FirstOrDefault(r => r.Name == Constants.Role.Admin.ToString().ToLowerInvariant());
		if (adminUser.Roles.Contains(adminRole)) {
			user.AddRole(role);
			await dbContext.SaveChangesAsync();
        }
		return user;
	}

	public async Task<Constants.Role?> GetUserRole(User? user) {
		if (user?.Roles == null || user.Roles.Count == 0) return null;

		var roleValues = user.Roles
			.Select(r => Enum.TryParse<Constants.Role>(r.Name, ignoreCase: true, out var role) ? role : (Constants.Role?)null)
			.Where(r => r.HasValue)
			.Select(r => r!.Value)
			.ToList();

		return roleValues.Count > 0 ? roleValues.Max() : null;
	}

	public async Task DeleteNewUser(User user) {
		if (DateTime.UtcNow < user.CreatedAt.ToUniversalTime().AddMinutes(5)) {
			dbContext.Users.Remove(user);
			await dbContext.SaveChangesAsync();
		}
		return;
	}
}