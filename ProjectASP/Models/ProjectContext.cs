using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectASP.Models;

namespace ProjectASP.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(category =>
            {
                category.Property(c => c.CategoryID).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
            modelBuilder.Entity<Product>(product =>
            {
                product.Property(p => p.ProductID).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Account>(account =>
            {
                account.Property(a => a.AccountID).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<OrderDetail>(order =>
            {
                order.Property(o => o.OrderID).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Account>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a => a.Phone).IsUnique();
            modelBuilder.Entity<Cart>(c =>
            {
                c.Property(cart => cart.CartID).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Login>(login =>
            {
                login.Property(c => c.ID).ValueGeneratedOnAdd();
            });
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}
