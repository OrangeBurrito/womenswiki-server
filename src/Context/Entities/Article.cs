public class Article : BaseEntity {
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public ICollection<ArticleEdit> ArticleEdits { get; set; } = new List<ArticleEdit>();

    public string GenerateSlug(string slug) {
        return String.Join("_", slug.ToLower().Trim().Split(" "));
    }
}