namespace WomensWiki.Services;

public class ArticleDTO {
    public string Title { get; private set; }
    public string Content { get; private set; }

    public ArticleDTO(string title, string content) {
        Title = title;
        Content = content;
    }

    public class UpdateArticleDTO {
        public Guid Id { get; private set; }
        public string Content { get; private set; }

        public UpdateArticleDTO(Guid id, string content) {
            Id = id;
            Content = content;
        }
    }

    public class ArticleResult {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }

        public ArticleResult(Guid id, string title, string content) {
            Id = id;
            Title = title;
            Content = content;
        }
    }
}