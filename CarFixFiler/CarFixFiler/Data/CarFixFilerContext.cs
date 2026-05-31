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

        // Customer - unique index on Name (business key) for efficient lookups
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Name_Unique");

        // Car - configure shadow property for technical ID
        modelBuilder.Entity<Car>()
            .Property<Guid>("CustomerId")
            .IsRequired();

        modelBuilder.Entity<Car>()
            .Property<Guid>("Id")
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        // Car - unique index on LicensePlateNumber (business key) for efficient lookups
        modelBuilder.Entity<Car>()
            .HasIndex(c => c.LicensePlateNumber)
            .IsUnique()
            .HasDatabaseName("IX_Cars_LicensePlateNumber_Unique");

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

        // Service - unique index on composite business key (LicensePlateNumber, Date) for efficient lookups
        modelBuilder.Entity<Service>()
            .HasIndex(nameof(Service.LicensePlateNumber), nameof(Service.Date))
            .IsUnique()
            .HasDatabaseName("IX_Services_LicensePlateNumber_Date_Unique");

        // Service - index on Date for ordering queries
        modelBuilder.Entity<Service>()
            .HasIndex(nameof(Service.Date))
            .IsDescending()
            .HasDatabaseName("IX_Services_Date_Descending");

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

        // ServiceItem - unique index on composite business key for efficient lookups
        modelBuilder.Entity<ServiceItem>()
            .HasIndex(nameof(ServiceItem.LicensePlateNumber), nameof(ServiceItem.Date), nameof(ServiceItem.ItemName))
            .IsUnique()
            .HasDatabaseName("IX_ServiceItems_LicensePlateNumber_Date_ItemName_Unique");

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
    }
}
