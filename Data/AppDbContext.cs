using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MiniEcom.Models;

namespace MiniEcom.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductTag> ProductTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StockTransaction> StockTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwOrderSummary> VwOrderSummaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC0794B4E93C");

            entity.HasIndex(e => e.UserId, "IX_Addresses_UserId");

            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Label).HasMaxLength(100);
            entity.Property(e => e.Line1).HasMaxLength(400);
            entity.Property(e => e.Line2).HasMaxLength(400);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.RecipientName).HasMaxLength(200);
            entity.Property(e => e.State).HasMaxLength(150);

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Addresses_User");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carts__3214EC072C4850B3");

            entity.HasIndex(e => e.UserId, "UX_Carts_User").IsUnique();

            entity.Property(e => e.LastUpdated).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.User).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.UserId)
                .HasConstraintName("FK_Carts_User");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC0700465F7B");

            entity.HasIndex(e => e.CartId, "IX_CartItems_Cart");

            entity.HasIndex(e => e.ProductId, "IX_CartItems_Product");

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartItems_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0766A20299");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F6281C4E59").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC077B33F469");

            entity.HasIndex(e => e.OrderNumber, "IX_Orders_OrderNumber");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__CAC5E743084532B0").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.OrderPlacedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.PaymentId).HasMaxLength(100);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.ShippingCharge).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.OrderBillingAddresses)
                .HasForeignKey(d => d.BillingAddressId)
                .HasConstraintName("FK_Orders_BillAddr");

            entity.HasOne(d => d.ShippingAddress).WithMany(p => p.OrderShippingAddresses)
                .HasForeignKey(d => d.ShippingAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_ShipAddr");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_User");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC0777733569");

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_Order");

            entity.HasIndex(e => e.ProductId, "IX_OrderItems_Product");

            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("([UnitPrice]*[Quantity])", true)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderItems_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Product");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC070C32F2EC");

            entity.HasIndex(e => e.OrderId, "IX_Payments_Order");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentProvider).HasMaxLength(100);
            entity.Property(e => e.PaymentReference).HasMaxLength(200);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Payments_Order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07632E7917");

            entity.HasIndex(e => e.CategoryId, "IX_Products_Category");

            entity.HasIndex(e => e.Sku, "IX_Products_SKU");

            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0D99D9914A").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Mrp)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("MRP");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShortDescription).HasMaxLength(1000);
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Category");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07D832D71D");

            entity.HasIndex(e => e.ProductId, "IX_ProductImages_Product");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.FilePath).HasMaxLength(1000);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImages_Product");
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC07A3A8081B");

            entity.Property(e => e.Tag).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTags)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductTags_Product");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07A1B5FD65");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F69BF67AA8").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockTra__3214EC074061D1A8");

            entity.HasIndex(e => e.ProductId, "IX_StockTransactions_Product");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.RelatedEntityId).HasMaxLength(100);

            entity.HasOne(d => d.Product).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockTransactions_Product");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC076B494EE6");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4B85E2338").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053425A0082F").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.PasswordSalt).HasMaxLength(128);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK__UserRole__AF2760AD4B66B350");

            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoles_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_User");
        });

        modelBuilder.Entity<VwOrderSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_OrderSummary");

            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.OrderStatus).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
