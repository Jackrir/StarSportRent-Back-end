using DataAccessLayer.Models.Entyties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Models
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TypeOfItem> TypeOfItems  { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<TypeItem> TypeItems { get; set; }
        public DbSet<Maintenance> Maintenances  { get; set; }
        public DbSet<Booking> Bookings  { get; set; }
        public DbSet<ItemsInRent> ItemsInRents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
              .HasMany(q => q.Rents)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
              .HasMany(q => q.Bookings)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
               .HasMany(q => q.TypeOfItems)
               .WithOne(a => a.Category)
               .HasForeignKey(a => a.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TypeOfItem>()
               .HasMany(q => q.TypeItems)
               .WithOne(a => a.Type)
               .HasForeignKey(a => a.TypeId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
               .HasMany(q => q.Maintenances)
               .WithOne(a => a.Item)
               .HasForeignKey(a => a.ItemId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Item>()
               .HasMany(q => q.Bookings)
               .WithOne(a => a.Item)
               .HasForeignKey(a => a.ItemId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Item>()
               .HasMany(q => q.ItemsInRents)
               .WithOne(a => a.Item)
               .HasForeignKey(a => a.ItemId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Item>()
               .HasMany(q => q.TypeItems)
               .WithOne(a => a.Item)
               .HasForeignKey(a => a.ItemId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rent>()
               .HasMany(q => q.ItemsInRents)
               .WithOne(a => a.Rent)
               .HasForeignKey(a => a.RentId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemsInRent>()
                .HasKey(us => new { us.RentId, us.ItemId });
            modelBuilder.Entity<TypeItem>()
                .HasKey(us => new { us.TypeId, us.ItemId });
        }
    }
}
