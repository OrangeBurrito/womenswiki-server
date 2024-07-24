namespace WomensWiki.Contracts;

public record CreateArticleResponse(Guid Id, DateTimeOffset CreatedAt, string Title, string Content);