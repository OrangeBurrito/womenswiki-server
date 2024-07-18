using Microsoft.EntityFrameworkCore;
using WomensWiki.Domain;

namespace WomensWiki.Common;

public class AppDbContext : DbContext {
    public DbSet<Article> Articles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}