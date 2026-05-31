using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CarFixFiler.Data.ValueObjects;

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
        // Configure value converters for ID value objects
        var customerIdConverter = new ValueConverter<CustomerId, Guid>(
            v => v.Value,
            v => new CustomerId(v));

        var carIdConverter = new ValueConverter<CarId, Guid>(
            v => v.Value,
            v => new CarId(v));

        var serviceIdConverter = new ValueConverter<ServiceId, Guid>(
            v => v.Value,
            v => new ServiceId(v));

        var serviceItemIdConverter = new ValueConverter<ServiceItemId, Guid>(
            v => v.Value,
            v => new ServiceItemId(v));

        // Customer - configure shadow property for technical ID as primary key
        modelBuilder.Entity<Customer>()
            .Property<CustomerId>("Id")
            .HasConversion(customerIdConverter)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Customer>()
            .HasKey("Id");

        // Customer - Name as alternate key (creates unique index automatically)
        modelBuilder.Entity<Customer>()
            .HasAlternateKey(c => c.Name);

        // Car - configure shadow property for technical ID as primary key
        modelBuilder.Entity<Car>()
            .Property<CarId>("Id")
            .HasConversion(carIdConverter)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Car>()
            .HasKey("Id");

        modelBuilder.Entity<Car>()
            .Property<CustomerId>("CustomerId")
            .HasConversion(customerIdConverter)
            .IsRequired();

        // Car - LicensePlateNumber as alternate key (creates unique index automatically)
        modelBuilder.Entity<Car>()
            .HasAlternateKey(c => c.LicensePlateNumber);

        // Service - configure shadow property for technical ID as primary key
        modelBuilder.Entity<Service>()
            .Property<ServiceId>("Id")
            .HasConversion(serviceIdConverter)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Service>()
            .HasKey("Id");

        modelBuilder.Entity<Service>()
            .Property<CarId>("CarId")
            .HasConversion(carIdConverter)
            .IsRequired();

        // Service - composite alternate key on (LicensePlateNumber, Date) - creates unique index automatically
        modelBuilder.Entity<Service>()
            .HasAlternateKey(nameof(Service.LicensePlateNumber), nameof(Service.Date));

        // Service - index on Date for ordering queries
        modelBuilder.Entity<Service>()
            .HasIndex(nameof(Service.Date))
            .IsDescending();

        // ServiceItem - configure shadow property for technical ID as primary key
        modelBuilder.Entity<ServiceItem>()
            .Property<ServiceItemId>("Id")
            .HasConversion(serviceItemIdConverter)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ServiceItem>()
            .HasKey("Id");

        modelBuilder.Entity<ServiceItem>()
            .Property<ServiceId>("ServiceId")
            .HasConversion(serviceIdConverter)
            .IsRequired();

        // ServiceItem - composite alternate key on (LicensePlateNumber, Date, ItemName) - creates unique index automatically
        modelBuilder.Entity<ServiceItem>()
            .HasAlternateKey(nameof(ServiceItem.LicensePlateNumber), nameof(ServiceItem.Date), nameof(ServiceItem.ItemName));

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
