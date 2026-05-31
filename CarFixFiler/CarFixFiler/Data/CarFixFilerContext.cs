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
        // Customer - configure shadow property for technical ID
        modelBuilder.Entity<Customer>()
            .Property<Guid>("Id")
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        // Car - configure shadow property for technical ID
        modelBuilder.Entity<Car>()
            .Property<Guid>("CustomerId")
            .IsRequired();

        modelBuilder.Entity<Car>()
            .Property<Guid>("Id")
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        // Service - configure shadow property for technical ID
        modelBuilder.Entity<Service>()
            .Property<Guid>("CarId")
            .IsRequired();

        modelBuilder.Entity<Service>()
            .Property<Guid>("Id")
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Service>()
            .HasKey("Id");

        modelBuilder.Entity<Service>()
            .HasIndex(nameof(Service.LicensePlateNumber), nameof(Service.Date))
            .IsUnique()
            .HasDatabaseName("IX_Services_BusinessKey");

        modelBuilder.Entity<Service>()
            .HasIndex(nameof(Service.Date))
            .IsDescending();

        // ServiceItem - configure shadow property for technical ID
        modelBuilder.Entity<ServiceItem>()
            .Property<Guid>("ServiceId")
            .IsRequired();

        modelBuilder.Entity<ServiceItem>()
            .Property<Guid>("Id")
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ServiceItem>()
            .HasKey("Id");

        modelBuilder.Entity<ServiceItem>()
            .HasIndex(nameof(ServiceItem.LicensePlateNumber), nameof(ServiceItem.Date), nameof(ServiceItem.ItemName))
            .IsUnique()
            .HasDatabaseName("IX_ServiceItems_BusinessKey");

        // Relationships
        modelBuilder.Entity<Service>()
            .HasMany(s => s.ServiceItems)
            .WithOne(si => si.Service)
            .HasForeignKey("ServiceId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Car>()
            .HasMany(c => c.Services)
            .WithOne(s => s.Car)
            .HasForeignKey("CarId")
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Cars)
            .WithOne(c => c.Customer)
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraints on business keys
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX_Customers_BusinessKey");

        modelBuilder.Entity<Car>()
            .HasIndex(c => c.LicensePlateNumber)
            .IsUnique()
            .HasDatabaseName("IX_Cars_BusinessKey");
    }
}
