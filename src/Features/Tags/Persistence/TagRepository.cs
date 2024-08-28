using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Common.Interfaces;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Tags.Persistence;

public class TagRepository(AppDbContext dbContext) : IRepository {
    public async Task<Tag?> GetTag(string name) {
        return await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tag?> GetFullTag(string name) {
        return await dbContext.Tags.Include(t => t.ParentTags).Include(t => t.Articles).FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<List<Tag>> GetTags(int limit, int offset, bool descending) {
        var query = descending ?
            dbContext.Tags.OrderByDescending(t => t.CreatedAt) :
            dbContext.Tags.OrderBy(t => t.CreatedAt);

        return await query
            .Include(t => t.Articles)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetSubtags(Tag tag, int limit, int offset, bool descending) {
        var query = descending ?
            dbContext.Tags.OrderByDescending(t => t.CreatedAt) :
            dbContext.Tags.OrderBy(t => t.CreatedAt);

        return await query
            .Where(t => t.ParentTags.Any(pt => pt.Name == tag.Name))
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetNestedSubtags(Tag tag) {
        var subtags = new List<Tag>();
        var directSubtags = await GetSubtags(tag, int.MaxValue, 0, false);
        subtags.AddRange(directSubtags);

        foreach (var subtag in directSubtags) {
            var nestedSubtags = await GetNestedSubtags(subtag);
            subtags.AddRange(nestedSubtags);
        }

        return subtags;
    }

    public async Task<List<Tag>> GetMatchingTags(List<string> tags) {
        return await dbContext.Tags.Where(t => tags.Contains(t.Name)).ToListAsync();
    }

    public async Task<Tag> CreateTag(string name, Tag? parentTag) {
        var tag = Tag.Create(name, parentTag);
        await dbContext.Tags.AddAsync(tag);
        await dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateTag(Tag tag, Tag parentTag) {
        tag.Update(parentTag);
        await dbContext.SaveChangesAsync();
        return tag;
    }
}