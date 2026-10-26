using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Fluent_Configuration
{
    public class Role_Config : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            //Change Name
            builder.ToTable("Roles");

            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.NormalizedName).HasMaxLength(100);
            builder.Property(p => p.ConcurrencyStamp).HasMaxLength(800);
        }
    }
}
