namespace Infrastructure.Persistence.Configurations;

using Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Property(x => x.FirstName)
            .HasMaxLength(100);

        builder
            .Property(x => x.LastName)
            .HasMaxLength(100);

        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(15);
    }
}