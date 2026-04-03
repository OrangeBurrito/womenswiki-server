using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WomensWiki.Domain;
using WomensWiki.Domain.Users;

namespace WomensWiki.Features.Users.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User> {
	public void Configure(EntityTypeBuilder<User> builder) {
		builder.HasKey(u => u.Id);
		
		builder.Property(u => u.Username)
			.IsRequired()
			.HasMaxLength(20);
			
		builder.Property(u => u.Email)
			.IsRequired()
			.HasMaxLength(255);
			
		builder.HasIndex(u => u.Username)
			.IsUnique();
			
		builder.HasIndex(u => u.Email)
			.IsUnique();

		builder.HasMany(u => u.Roles)
			.WithMany()
			.UsingEntity(j => {
				j.ToTable("user_roles");
				j.Property("UserId").HasColumnName("user_id");
				j.Property("RolesId").HasColumnName("role_id");
			});
	}
}
