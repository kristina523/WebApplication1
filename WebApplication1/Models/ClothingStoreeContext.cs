using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.WebApplication1.Models;

namespace WebApplication1.Models;

public partial class ClothingStoreeContext : DbContext
{
    public ClothingStoreeContext()
    {
    }

    public ClothingStoreeContext(DbContextOptions<ClothingStoreeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<Catalog> Catalogs { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<PosOrder> PosOrders { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<CartItems> CartItems { get; set; } // Add this line

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=krolik\\SQLEXPRESS;Initial Catalog=ClothingStoree;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("PK__Admins__89472E95818CA179");

            entity.HasIndex(e => e.Login, "UQ__Admins__7838F272FAF12505").IsUnique();

            entity.Property(e => e.IdAdmin).HasColumnName("id_admin");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        // ... Other entity configurations remain unchanged ...

        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__6A3A7DCD2EFBCF81"); // Set primary key

            entity.Property(e => e.CartItemId).HasColumnName("cart_item_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Product) // Assuming you have a navigation property
                .WithMany() // Assuming a Catalog can have many CartItems
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Catalog");

            entity.HasOne(d => d.User) // Assuming you have a navigation property
                .WithMany() // Assuming a User can have many CartItems
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
