using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //DBSets (one per table)───────────────────────────────────────────────
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

        //Fluent API Configuration ───────────────────────────────────────────────
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Category ───────────────────────────────────────────────
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Description).HasMaxLength(100);
                entity.HasIndex(c => c.Name).IsUnique(); // no duplicate category name

            });

            //Supplier───────────────────────────────────────────────
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(50);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(75);
                entity.Property(s => s.Phone).HasMaxLength(50);
                entity.HasIndex(s => s.Email).IsUnique();
            });

            //Product───────────────────────────────────────────────
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(p => p.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                //one category -> many product
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);//prevent category delete if product exist

                //one supplier -> many product
                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
            // ── ProductReview ─────────────────────────────────────────────────────
            modelBuilder.Entity<ProductReview>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Comment).HasMaxLength(1000);
                entity.Property(r => r.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                //one product -> many review
                entity.HasOne(r => r.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);//delete product delete its reviews
            });

        }

    }
}
