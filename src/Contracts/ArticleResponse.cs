namespace WomensWiki.Contracts;

public record ArticleResponse(Guid Id, DateTimeOffset CreatedAt, string Title, string Content);