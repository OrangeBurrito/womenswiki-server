using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WomensWiki.Domain;

namespace WomensWiki.Features.Users.Persistence;

public class RoleConfiguration : IEntityTypeConfiguration<Role> {
	public void Configure(EntityTypeBuilder<Role> builder) {
		builder.HasKey(r => r.Id);
		
		builder.Property(r => r.Name)
			.IsRequired()
			.HasMaxLength(50);
			
		builder.HasIndex(r => r.Name)
			.IsUnique();
	}
}
