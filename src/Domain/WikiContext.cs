public class WikiContext : DbContext {
    public DbSet<Article> Articles { get; set; }
    public DbSet<Revision> Revisions { get; set; }
    public DbSet<User> Users { get; set; }
    
    public WikiContext(DbContextOptions<WikiContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Article>()
            .HasOne(a => a.LatestRevision)
            .WithOne(a => a.Article)
            .HasForeignKey<Article>(p => p.LatestRevisionId)
            .IsRequired(false);
    }
}