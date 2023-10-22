public class Article : BaseEntity {
    public string Title { get; private set; }

    public Article(string title) {
        Title = title;
    }

    public void Update(string title) {
        Title = title;
    }
}