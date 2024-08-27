using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Common.Interfaces;
using WomensWiki.Domain;

namespace WomensWiki.Features.Users.Persistence;

public class UserRepository(AppDbContext dbContext) : IRepository {
    public async Task<User?> GetUserByUsername(string username) {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}