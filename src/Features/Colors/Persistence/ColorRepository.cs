using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Domain.Colors;

namespace WomensWiki.Features.Colors.Persistence;

public class ColorRepository(AppDbContext dbContext) : IColorRepository {
    public async Task<Color?> GetColor(string name) {
        var color = dbContext.Colors.FirstOrDefaultAsync(c => c.Name == name);
        
        return await color;
    }

    public async Task<Color?> CreateColor(string name, string value) {
        var color = Color.Create(name, value);

        await dbContext.Colors.AddAsync(color);
        await dbContext.SaveChangesAsync();
        return color;
    }
}