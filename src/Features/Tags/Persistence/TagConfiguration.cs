using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Tags.Persistence;

public class TagConfiguration : IEntityTypeConfiguration<Tag> {
    public void Configure(EntityTypeBuilder<Tag> builder) {
        builder.HasMany(t => t.ParentTags).WithMany().UsingEntity(join => join.ToTable("ChildTags"));
    }
}