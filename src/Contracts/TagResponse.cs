using WomensWiki.Domain.Articles;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Contracts;

public record TagResponse(Guid Id, DateTimeOffset CreatedAt, string Name, List<Article>? Articles, List<Tag>? ParentTags) {
    public static TagResponse FromTag(Tag tag) {
        return new TagResponse(tag.Id, tag.CreatedAt, tag.Name, tag.Articles, tag.ParentTags);
    }
}