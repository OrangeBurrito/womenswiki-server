using WomensWiki.Domain.Articles;
using WomensWiki.Domain.Colors;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Contracts;

public record TagResponse(Guid Id, DateTimeOffset CreatedAt, string Name, Color Color, List<Article>? Articles, List<Tag>? ParentTags) {
    public static TagResponse FromTag(Tag tag) {
        return new TagResponse(tag.Id, tag.CreatedAt, tag.Name, tag.Color, tag.Articles, tag.ParentTags);
    }
}