using System;
using Microsoft.EntityFrameworkCore;

using Airport.Data.Models;

namespace Airport.Data.DatabaseContext
{
  public class AirportDbContext : DbContext
  {
    public DbSet<Airhostess> Airhostess { get; set; }
    public DbSet<Crew> Crew { get; set; }
    public DbSet<Departure> Departure { get; set; }
    public DbSet<Flight> Flight { get; set; }
    public DbSet<Pilot> Pilot { get; set; }
    public DbSet<Plane> Plane { get; set; }
    public DbSet<PlaneType> PlaneType { get; set; }
    public DbSet<Ticket> Ticket { get; set; }

    public AirportDbContext(DbContextOptions<AirportDbContext> options)
      : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Flight>().HasIndex(x => x.Number).IsUnique(true);
      modelBuilder.Entity<PlaneType>().HasIndex(x => x.Model).IsUnique(true);
    }
  }
}