using Microsoft.EntityFrameworkCore;
using WomensWiki.Domain;

namespace WomensWiki.Common;

public class AppDbContext : DbContext {
    public DbSet<Article> Articles { get; set; }
    public DbSet<Revision> Revisions { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Article>().HasKey(a => a.Id);
        modelBuilder.Entity<Article>().Property(a => a.Id).ValueGeneratedNever();
        modelBuilder.Entity<Article>()
            .HasMany(a => a.History)
            .WithOne(r => r.Article)
            .HasForeignKey(r => r.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Revision>().HasKey(r => r.Id);
        modelBuilder.Entity<Revision>().Property(r => r.Id).ValueGeneratedNever();
        modelBuilder.Entity<Revision>().Property(r => r.Summary).HasMaxLength(72).IsRequired(false);
        modelBuilder.Entity<Revision>()
            .HasOne(r => r.Article)
            .WithMany(a => a.History)
            .HasForeignKey(r => r.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}