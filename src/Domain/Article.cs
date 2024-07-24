namespace WomensWiki.Domain;

public class Article : Entity {
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public List<Revision> History { get; private set; } = new();
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static Article Create(string title, string content) {
        var article = new Article {
            Title = title,
            Content = content,
            Slug = GenerateSlug(title)
        };
        return article;
    }

    public void Update(User user, string content, string? summary = null) {
        var revision = Revision.Create(this, user, content, summary);
        History.Add(revision);
        Content = revision.Content;
        UpdatedAt = revision.CreatedAt;
    }

    private static string GenerateSlug(string title) {
        return title.ToLower().Replace(" ", "_");
    }
}