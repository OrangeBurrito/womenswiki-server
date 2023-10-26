public class ArticleDTO {
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }

    public ArticleDTO(Guid id, string title, string content) {
        Id = id;
        Title = title;
        Content = content;
    }
}