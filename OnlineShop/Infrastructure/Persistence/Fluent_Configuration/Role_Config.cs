using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
