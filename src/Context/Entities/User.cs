public class User : BaseEntity {
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public ICollection<Revision> ArticleEdits { get; set; } = new List<Revision>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}