using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Fluent_Configuration
{
    public class User_Config : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //Change Name
            builder.ToTable("Users");

            //09126668880
            builder.Property(p => p.PhoneNumber).HasMaxLength(11);

            builder.Property(p => p.ConcurrencyStamp).HasMaxLength(800);
            builder.Property(p => p.SecurityStamp).HasMaxLength(800);
            builder.Property(p => p.PasswordHash).HasMaxLength(800);
            builder.Property(p => p.UserName).HasMaxLength(200);
            builder.Property(p => p.NormalizedUserName).HasMaxLength(200);
            builder.Property(p => p.Email).HasMaxLength(200);
            builder.Property(p => p.NormalizedEmail).HasMaxLength(200);

        }
    }
}
