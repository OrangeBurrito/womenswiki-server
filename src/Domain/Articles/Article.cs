namespace WomensWiki.Domain.Articles;
using Tag = WomensWiki.Domain.Tags.Tag;

public class Article : Entity {
    public string Title { get; private set; } = null!;
    public string? LatestVersion { get; private set; } = null!;
    public List<Tag> Tags { get; private set; } = [];
    public DateTimeOffset? UpdatedAt { get; private set; }
    public List<Revision> History { get; private set; } = [];

    public static Article Create(string title) {
        return new Article {
            Title = FormatTitle(title)
        };
    }

    public void Update(User user, string content, string? summary = null) {
        var revision = Revision.Create(this, user, content, summary);
        History.Add(revision);
        if (LatestVersion != null) {
            UpdatedAt = revision.CreatedAt;
        }
        LatestVersion = revision.Content;
    }

    public void UpdateTags(List<Tag> tags) {
        Tags.Clear();
        Tags.AddRange(tags);
    }

    public static string FormatTitle(string title) {
        return string.Join("_", title.Split(" ", StringSplitOptions.RemoveEmptyEntries));
    }
}