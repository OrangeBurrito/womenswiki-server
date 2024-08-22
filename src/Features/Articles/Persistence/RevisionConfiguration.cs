using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WomensWiki.Domain;

namespace WomensWiki.Features.Articles.Persistence;

public class RevisionConfiguration : IEntityTypeConfiguration<Revision> {
    public void Configure(EntityTypeBuilder<Revision> builder){
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();
        builder.Property(r => r.Summary).HasMaxLength(72).IsRequired(false);
        builder.HasOne(r => r.Article)
            .WithMany(a => a.History)
            .HasForeignKey(r => r.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}