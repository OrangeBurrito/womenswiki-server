public class Article : BaseEntity {
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ICollection<Revision> Revisions { get; set; } = null!;
    public Guid? LatestRevisionId { get; set; }
    public Revision? LatestRevision { get; set; }

    public static string GenerateSlug(string slug) {
        return String.Join("_", slug.ToLower().Trim().Split(" "));
    }
}