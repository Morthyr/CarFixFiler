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
        modelBuilder.Entity<Service>()
            .HasMany(s => s.ServiceItems)
            .WithOne(si => si.Service);

        modelBuilder.Entity<Car>()
            .HasMany(c => c.Services)
            .WithOne(s => s.Car);
        
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Cars)
            .WithOne(s => s.Customer);
    }
}
