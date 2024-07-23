namespace WomensWiki.Domain;

public class Article : Entity {
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;

    public static Article Create(string title, string content) {
        var article = new Article {
            Title = title,
            Content = content
        };
        return article;
    }

    public static Article Update(Article article, string title, string content) {
        article.Title = title;
        article.Content = content;
        return article;
    }
}