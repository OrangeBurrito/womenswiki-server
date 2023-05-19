using Microsoft.EntityFrameworkCore;

public class WikiContext : DbContext {
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleEdit> ArticleEdits { get; set; } = null!;

    public WikiContext(DbContextOptions<WikiContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Article>(entity => {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Title).IsRequired();
            entity.HasIndex(e => e.Title).IsUnique();
        });
    }

    public override int SaveChanges() {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries) {
            if (entry.Entity is Article article) {
                if (entry.State == EntityState.Added) {
                    article.Slug = article.GenerateSlug(article.Title);
                }
            }
        }
        return base.SaveChanges();
    }
}