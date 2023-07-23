using Microsoft.EntityFrameworkCore;
using ReportTest.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ReportTest.Data
{
    public class AppDbContext : DbContext   
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
