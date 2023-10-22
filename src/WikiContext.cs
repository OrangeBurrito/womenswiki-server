using Microsoft.EntityFrameworkCore;

public class WikiContext : DbContext {
    public DbSet<Article> Articles { get; set; }
    
    public WikiContext(DbContextOptions<WikiContext> options) : base(options) { }
}