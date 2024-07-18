namespace WomensWiki.Domain;

public class Article : Entity {
    public string Title { get; private set; }

    public Guid? LatestRevisionId { get; private set; }
    public Revision? LatestRevision { get; private set; }

    public Article(string title) {
        Title = title;
    }

    public void Update(Revision revision) {
        LatestRevision = revision;
        LatestRevisionId = revision.Id;
    }
}