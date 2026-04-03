using WomensWiki.Common;
using WomensWiki.Domain;

namespace WomensWiki.Infrastructure.Persistence;

public static class AppDbContextSeed {
	public static async Task SeedInitialDataAsync(AppDbContext dbContext) {
		if (!dbContext.Roles.Any()) {
			dbContext.Roles.AddRange(
				Role.Create(Constants.Role.User),
				Role.Create(Constants.Role.Editor),
				Role.Create(Constants.Role.Admin)
			);
			await dbContext.SaveChangesAsync();
		}
    }
}