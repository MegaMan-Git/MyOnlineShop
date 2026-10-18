using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Fluent_Configuration
{
    public class Order_Config : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Order 1 <--> N OrderItem
            builder
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                 //If the order is deleted, then the order items will be deleted.
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .HasDefaultValue(OrderStatus.AwaitingPayment);

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("(getutcdate())");
        }
    }
}
