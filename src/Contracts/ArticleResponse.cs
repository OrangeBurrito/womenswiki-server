using WomensWiki.Domain;

namespace WomensWiki.Contracts;

public record ArticleResponse(Guid Id, DateTimeOffset? UpdatedAt, string Title, string Content) {
    public static ArticleResponse FromArticle(Article article) {
        return new ArticleResponse(article.Id, article.UpdatedAt, article.Title, article.Content);
    }
}