public class Article : BaseEntity {
    public string Title { get; set; } = "";
    public ICollection<ArticleEdit> ArticleEdits { get; set; } = new List<ArticleEdit>();
}