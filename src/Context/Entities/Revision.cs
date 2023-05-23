public class Revision : BaseEntity {
    public string Content { get; set; } = "";
    public Guid ArticleId { get; set; }
}