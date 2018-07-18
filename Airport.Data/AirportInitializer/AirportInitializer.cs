using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Airport.Data.MockData;
using Airport.Data.Models;
using Airport.Data.DatabaseContext;


namespace Airport.Data.AirportInitializer
{
  public class AirportInitializer
  {
    AirportDbContext _dbContext;
    DataSource _dataSource;

    public AirportInitializer(AirportDbContext dbContext, DataSource dataSource)
    {
      _dbContext = dbContext;
      _dataSource = dataSource;
    }

    public async Task Seed()
    {
      await _dbContext.Database.BeginTransactionAsync();

      await Seed<Pilot>();
      await Seed<Crew>();
      await Seed<Airhostess>();
      await Seed<PlaneType>();
      await Seed<Plane>();
      await Seed<Flight>();
      await Seed<Departure>();
      await Seed<Ticket>();

      _dbContext.Database.CommitTransaction();
    }

    public async Task AntiSeed()
    {
      await _dbContext.Database.BeginTransactionAsync();

      await AntiSeed<Ticket>();
      await AntiSeed<Departure>();
      await AntiSeed<Flight>();
      await AntiSeed<Plane>();
      await AntiSeed<PlaneType>();
      await AntiSeed<Airhostess>();
      await AntiSeed<Crew>();
      await AntiSeed<Pilot>();

      _dbContext.Database.CommitTransaction();
    }

    public async Task Seed<TEntity>() where TEntity : class
    {
      var dbSet = _dbContext.Set<TEntity>();
      if (!await dbSet.AnyAsync())
      {
        Console.WriteLine($"Seeding {typeof(TEntity).Name} table . . .");
        await IdentityInsert<TEntity>(true);
        await Reseed<TEntity>();

        var seedData = _dataSource.Get<TEntity>();
        await dbSet.AddRangeAsync(seedData);
        await _dbContext.SaveChangesAsync();

        await IdentityInsert<TEntity>(false);
      }
    }

    public async Task AntiSeed<TEntity>() where TEntity : class
    {
      var dbSet = _dbContext.Set<TEntity>();
      if (await dbSet.AnyAsync())
      {
        Console.WriteLine($"Anti seeding {typeof(TEntity).Name} table . . .");
        var query = $"DELETE FROM {typeof(TEntity).Name}";

        await _dbContext.Database.ExecuteSqlCommandAsync(query);
        await Reseed<TEntity>();
      }
    }

    private async Task IdentityInsert<TEntity>(bool enable)
    {
      var @switch = enable ? "ON" : "OFF";
      var query = $"SET IDENTITY_INSERT {typeof(TEntity).Name} {@switch};";
      await _dbContext.Database.ExecuteSqlCommandAsync(query);
    }

    private async Task Reseed<TEntity>()
    {
      var query = $"DBCC CHECKIDENT ('{typeof(TEntity).Name}', RESEED, 1);";
      await _dbContext.Database.ExecuteSqlCommandAsync(query);
    }
  }
}