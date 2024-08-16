namespace WomensWiki.Contracts;
using Tag = WomensWiki.Domain.Tags.Tag;

public record CreateArticleResponse(Guid Id, DateTimeOffset CreatedAt, string Title, string Content, string Slug, List<Tag>? Tags = null);