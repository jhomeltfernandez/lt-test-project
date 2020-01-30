using JetBrains.Annotations;
using LTTestProject.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.DataAccess
{
    public class LTTestProjectDbContext : DbContext
    {
        public LTTestProjectDbContext(DbContextOptions<LTTestProjectDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(_ => new { _.Id });

            modelBuilder.Entity<Product>()
                .HasOne(_ => _.Category)
                .WithMany(_ => _.Products)
                .HasForeignKey(_ => _.CategoryId);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }

}
