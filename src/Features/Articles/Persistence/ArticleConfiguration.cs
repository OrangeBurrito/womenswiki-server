using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WomensWiki.Domain.Articles;

namespace WomensWiki.Features.Articles.Persistence;

public class ArticleConfiguration : IEntityTypeConfiguration<Article> {
    public void Configure(EntityTypeBuilder<Article> builder) {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();
        
        builder.HasMany(a => a.History)
            .WithOne(r => r.Article)
            .HasForeignKey(r => r.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(left => left.Tags)
            .WithMany(right => right.Articles)
            .UsingEntity(join => join.ToTable("article_tags"));
    }
}