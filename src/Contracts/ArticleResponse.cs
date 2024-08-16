using WomensWiki.Domain.Articles;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Contracts;

public record ArticleResponse(Guid Id, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, string Title, string Content, string Slug, List<Tag> Tags) {
    public static ArticleResponse FromArticle(Article article) {
        return new ArticleResponse(article.Id, article.CreatedAt, article.UpdatedAt, article.Title, article.Content, article.Slug, article.Tags);
    }
}