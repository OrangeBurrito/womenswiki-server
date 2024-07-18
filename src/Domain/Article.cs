namespace WomensWiki.Domain;

<<<<<<< HEAD
public class Article : BaseEntity {
    public string Title { get; private set; }

    public Guid? LatestRevisionId { get; private set; }
    public Revision? LatestRevision { get; private set; }

    public Article(string title) {
        Title = title;
    }

    public void Update(Revision revision) {
        LatestRevision = revision;
        LatestRevisionId = revision.Id;
=======
public class Article : Entity {
    public string Title { get; private set; }
    public string Content { get; private set; }

    public Article(string title, string content) {
        Title = title;
        Content = content;
>>>>>>> 5097f8f (refactor: use vertical slices)
    }
}