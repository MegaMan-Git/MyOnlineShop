using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Fluent_Configuration
{
    public class Payment_Config : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Payment 1 <--> 1 Order
            builder
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p=>p.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasIndex(p => p.OrderId)
                .IsUnique();
        }
    }
}
