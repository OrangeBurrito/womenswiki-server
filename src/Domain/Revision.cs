namespace WomensWiki.Domain;

public class Revision : Entity {

    public Guid ArticleId { get; init; }
    public Article Article { get; private set; } = null!;
    public User Author { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public string? Summary { get; private set; }

    public static Revision Create(Article article, User author, string content, string? summary = null) {
        var revision = new Revision {
            Article = article,
            ArticleId = article.Id,
            Author = author,
            Content = content,
            Summary = summary
        };
        return revision;
    }
}