namespace WomensWiki.Contracts;

public record TagResponse(Guid Id, DateTimeOffset CreatedAt, string Name);