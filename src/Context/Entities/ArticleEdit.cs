public class ArticleEdit : BaseEntity {
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public Article Article { get; set; } = null!;
    public User User { get; set; } = null!;
}