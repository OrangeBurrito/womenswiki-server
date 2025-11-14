using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Domain;

namespace WomensWiki.Features.Users.Persistence;

public class UserRepository(AppDbContext dbContext) : IUserRepository {
    public async Task<User> CreateUser(string username, string email, string passwordHash)
    {
        var user = User.Create(username, email, passwordHash);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

	public async Task SetRefreshToken(User user, string token, DateTime expiry) {
		user.SetRefreshToken(token, expiry);
		dbContext.Users.Update(user);
		await dbContext.SaveChangesAsync();
		return;
	}

    public async Task<User?> GetUserByUsername(string username) {
		return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
	}
}