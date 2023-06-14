using Microsoft.EntityFrameworkCore;

public class WikiContext : DbContext {
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Revision> Revisions { get; set; } = null!;

    public WikiContext(DbContextOptions<WikiContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Article>(entity => {
            entity.Property(e => e.Title).IsRequired();
            entity.HasIndex(e => e.Title).IsUnique();
            entity.HasMany(a => a.Revisions).WithOne().HasForeignKey(r => r.ArticleId).IsRequired();
            entity.HasOne(a => a.LatestRevision).WithOne().HasForeignKey<Article>(a => a.LatestRevisionId);
        });
    }
}