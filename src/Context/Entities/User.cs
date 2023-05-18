public class User : BaseEntity {
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public ICollection<ArticleEdit> ArticleEdits { get; set; } = new List<ArticleEdit>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}