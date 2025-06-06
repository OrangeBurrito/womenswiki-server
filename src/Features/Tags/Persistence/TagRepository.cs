using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Domain.Colors;
using WomensWiki.Domain.Tags;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Tags.Persistence;

public class TagRepository(AppDbContext dbContext) : ITagRepository {
    public async Task<Tag?> GetTag(string name) {
        return await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tag?> GetFullTag(string name) {
        return await dbContext.Tags
        .Include(t => t.ParentTags).ThenInclude(pt => pt.Color).AsNoTracking()
        .Include(t => t.Articles).AsNoTracking()
        .Include(t => t.Color).AsNoTracking()
        .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<List<Tag>> GetTags(int limit, int offset, bool descending) {
        var query = descending ?
            dbContext.Tags.OrderByDescending(t => t.CreatedAt) :
            dbContext.Tags.OrderBy(t => t.CreatedAt);

        return await query
            .Include(t => t.Articles)
            .Include(t => t.Color).AsNoTracking()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public IQueryable<Tag> SortTags(int limit, int offset, bool descending) {
        var query = descending ?
            dbContext.Tags.OrderByDescending(t => t.Name) :
            dbContext.Tags.OrderBy(t => t.Name);

        return query
            .Include(t => t.Articles)
            .Skip(offset)
            .Take(limit);
    }

    public async Task<List<Tag>> GetSubtags(IQueryable<Tag> query, Tag tag) {
        return await query
            .Where(t => t.ParentTags.Any(pt => pt.Name == tag.Name))
            .Include(t => t.Color).AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<Tag>> GetSubtags(Tag tag) {
        return await GetSubtags(dbContext.Tags, tag);
    }

    public async Task<TagTree?> GetNestedSubtags(Tag tag, string? match = null) {
        var tagTree = new TagTree(tag);
        var directSubtags = await GetSubtags(tag);

        foreach (var subtag in directSubtags) {
            var nestedSubtags = await GetNestedSubtags(subtag);
            tagTree.AddSubtags(nestedSubtags);
            if (match != null && subtag.Name == match) {
                return null;
            }
        }
        return tagTree;
    }

    public async Task<List<Tag>> GetMatchingTags(List<string> tags) {
        return await dbContext.Tags.Where(t => tags.Contains(t.Name)).ToListAsync();
    }

    public async Task<Tag> CreateTag(string name, Color color, Tag? parentTag) {
        var tag = Tag.Create(name, color, parentTag);
        await dbContext.Tags.AddAsync(tag);
        await dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateTag(Tag tag, Tag parentTag) {
        tag.Update(parentTag);
        await dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateTagColor(Tag tag, Color color) {
        tag.UpdateColor(color);
        await dbContext.SaveChangesAsync();
        return tag;
    }
}