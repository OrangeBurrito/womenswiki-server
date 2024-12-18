namespace WomensWiki.Features.Tags.Persistence;

using WomensWiki.Common.Interfaces;
using WomensWiki.Domain.Tags;
using Tag = WomensWiki.Domain.Tags.Tag;

public interface ITagRepository : IRepository {
    Task<Tag?> GetTag(string name);
    Task<Tag?> GetFullTag(string name);
    Task<List<Tag>> GetTags(int limit, int offset, bool descending);
    IQueryable<Tag> SortTags(int limit, int offset, bool descending);
    Task<List<Tag>> GetSubtags(IQueryable<Tag> query, Tag tag);
    Task<List<Tag>> GetSubtags(Tag tag);
    Task<TagTree?> GetNestedSubtags(Tag tag, string? match = null);
    Task<List<Tag>> GetMatchingTags(List<string> tags);
    Task<Tag> CreateTag(string name, Tag? parentTag);
    Task<Tag> UpdateTag(Tag tag, Tag parentTag);
}