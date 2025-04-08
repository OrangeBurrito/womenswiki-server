using Microsoft.EntityFrameworkCore;
using WomensWiki.Domain;
using WomensWiki.Domain.Articles;
using WomensWiki.Domain.Colors;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Common;

public class AppDbContext : DbContext {
    public DbSet<Article> Articles { get; set; }
    public DbSet<Revision> Revisions { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Color> Colors { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}