using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
    public class SalesContext:DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.connectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigProductModel(modelBuilder);

            ConfigCustomerModel(modelBuilder);

            ConfigStoreModel(modelBuilder);

            ConfigSaleModel(modelBuilder);
        }

        private void ConfigSaleModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sale>()
                .HasKey(x => x.SaleId);

            modelBuilder
                .Entity<Sale>()
                .Property(x => x.Date)
                .HasDefaultValueSql("GETDATE()");

        }

        private void ConfigStoreModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Store>()
                .HasKey(x => x.StoreId);

            modelBuilder
                .Entity<Store>()
                .Property(x => x.Name)
                .HasMaxLength(80)
                .IsUnicode(true);

            modelBuilder
                .Entity<Store>()
                .HasMany(s => s.Sales)
                .WithOne(x => x.Store);
        }

        private void ConfigCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasKey(x => x.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsUnicode(true);

            modelBuilder
                .Entity<Customer>()
                .Property(x => x.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

            modelBuilder
                .Entity<Customer>()
                .HasMany(s => s.Sales)
                .WithOne(c => c.Customer);
        }

        private void ConfigProductModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasKey(x => x.ProductId);

            modelBuilder
                .Entity<Product>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode(true);

            modelBuilder
                .Entity<Product>()
                .HasMany(s => s.Sales)
                .WithOne(p => p.Product);

            modelBuilder
                .Entity<Product>()
                .Property(x => x.Description)
                .HasMaxLength(250)
                .IsUnicode(true);

            modelBuilder
                .Entity<Product>()
                .Property(x => x.Description)
                .HasDefaultValue("No description");
        }
    }
}
