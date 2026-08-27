using System;
using System.Collections.Generic;
using Application.Entities;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Fluent_Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public partial class OnlineShopContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{

    public OnlineShopContext(DbContextOptions<OnlineShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Default Scaffold FluentApi
        modelBuilder.UseCollation("Persian_100_CI_AS_SC_UTF8");

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carts__3214EC07BDAD2BEC");

            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC07063D532A");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07F3928026");

            entity.HasIndex(e => e.Title, "UQ_Categories_Title").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07E0DCA0B9");

            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07DBC0588D");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC074ABBC828");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Wating");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07449861B3");

            entity.HasIndex(e => e.ProductName, "IX_ProductName");

            entity.Property(e => e.ProductName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
        #endregion

        
        modelBuilder.ApplyConfiguration(new Cart_Config());
        modelBuilder.ApplyConfiguration(new Order_Config());
        modelBuilder.ApplyConfiguration(new Payment_Config());
        modelBuilder.ApplyConfiguration(new Product_Config());
        modelBuilder.ApplyConfiguration(new Role_Config());
        modelBuilder.ApplyConfiguration(new User_Config());

        // Add Relation User With Tables
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Cart)
            .WithOne()
            .HasForeignKey<Cart>(c => c.UserId);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(a => a.orders)
            .WithOne()
            .HasForeignKey(o => o.UserId);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
