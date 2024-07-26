using WomensWiki.Domain.Articles;

namespace WomensWiki.Contracts;

public record ArticleResponse(Guid Id, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, string Title, string Content, string Slug) {
    public static ArticleResponse FromArticle(Article article) {
        return new ArticleResponse(article.Id, article.CreatedAt, article.UpdatedAt, article.Title, article.Content, article.Slug);
    }
}