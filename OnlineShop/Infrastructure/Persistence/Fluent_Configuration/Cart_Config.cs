using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Fluent_Configuration
{
    public class Cart_Config : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Cart 1 <--> N CartItem 
            builder
                .HasMany(ci => ci.CartItems)
                .WithOne(c => c.Cart)
                .HasForeignKey(ci => ci.CartId);

            builder
                .HasIndex(ci => ci.UserId)
                .IsUnique();
        }
    }
}
