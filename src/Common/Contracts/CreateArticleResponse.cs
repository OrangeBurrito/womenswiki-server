namespace WomensWiki.Common.Contracts;
using Tag = WomensWiki.Domain.Tags.Tag;

public record CreateArticleResponse(Guid Id, DateTimeOffset CreatedAt, string Title, string Content, List<Tag>? Tags = null);