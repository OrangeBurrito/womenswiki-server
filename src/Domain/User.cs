namespace WomensWiki.Domain;

public class User : BaseEntity {
    public string Username { get; private set; }
    public string Email { get; private set; }

    public User(string username, string email) {
        Username = username;
        Email = email;
    }
}