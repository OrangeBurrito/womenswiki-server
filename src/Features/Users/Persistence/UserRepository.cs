using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Domain;

namespace WomensWiki.Features.Users.Persistence;

public class UserRepository(AppDbContext dbContext) : IUserRepository {
    public async Task<User?> GetUserByUsername(string username) {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}