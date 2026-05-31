using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarFixFiler.Data;

public class CarFixFilerContext : DbContext
{
    public CarFixFilerContext (DbContextOptions<CarFixFilerContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Car { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Service> Services { get; set; } = default!;
    public DbSet<ServiceItem> ServiceItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Service -> ServiceItems relationship
        modelBuilder.Entity<Service>()
            .HasMany(s => s.ServiceItems)
            .WithOne(si => si.Service)
            .HasForeignKey(si => si.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Car -> Services relationship
        modelBuilder.Entity<Car>()
            .HasMany(c => c.Services)
            .WithOne(s => s.Car)
            .HasForeignKey(s => s.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Customer -> Cars relationship
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Cars)
            .WithOne(c => c.Customer)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add unique constraints on business keys
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Car>()
            .HasIndex(c => c.LicensePlateNumber)
            .IsUnique();

        modelBuilder.Entity<Service>()
            .HasIndex(s => new { s.LicensePlateNumber, s.Date })
            .IsUnique();

        modelBuilder.Entity<ServiceItem>()
            .HasIndex(si => new { si.LicensePlateNumber, si.Date, si.ItemName })
            .IsUnique();
    }
}
