using WomensWiki.Domain.Articles;

namespace WomensWiki.Domain.Tags;

public class Tag : Entity {
    public string Name { get; private set; } = null!;
    public List<Tag>? ParentTags { get; private set; } = new();
    public List<Article> Articles { get; private set; } = new();

    public static Tag Create(string name, List<Tag>? parentTags = null) {
        return new Tag {
            Name = name,
            ParentTags = parentTags,
        };
    }
    public static Tag Create(string name, Tag? parentTag = null) {
        return new Tag {
            Name = name,
            ParentTags = parentTag is not null ? new List<Tag> { parentTag } : new List<Tag>(),
        };
    }

    public void Update(Tag parentTags) {
        ParentTags.Add(parentTags);
    }

    public void AddArticle(Article article, Tag tag) {
        tag.Articles.Add(article);
    }
}