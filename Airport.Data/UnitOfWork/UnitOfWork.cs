using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airport.Data.MockData;
using Airport.Data.Repositories;
using Airport.Data.Models;

using Airport.Data.DatabaseContext;

namespace Airport.Data.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork, IDisposable
  {
    protected readonly AirportDbContext _dbContext;
    public IAirhostessRepository AirhostessRepository { get; }
    public ICrewRepository CrewRepository { get; }
    public IDepartureRepository DepartureRepository { get; }
    public IFlightRepository FlightRepository { get; }
    public IPilotRepository PilotRepository { get; }
    public IPlaneRepository PlaneRepository { get; }
    public IPlaneTypeRepository PlaneTypeRepository { get; }
    public ITicketRepository TicketRepository { get; }

    public UnitOfWork(
      AirportDbContext dbContext
    )
    {
      _dbContext = dbContext;
      AirhostessRepository = new AirhostessRepository(dbContext);
      CrewRepository = new CrewRepository(dbContext);
      DepartureRepository = new DepartureRepository(dbContext);
      FlightRepository = new FlightRepository(dbContext);
      PilotRepository = new PilotRepository(dbContext);
      PlaneRepository = new PlaneRepository(dbContext);
      PlaneTypeRepository = new PlaneTypeRepository(dbContext);
      TicketRepository = new TicketRepository(dbContext);
    }

    public int SaveChanges()
    {
      return _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
      return await _dbContext.SaveChangesAsync();
    }

    public IRepository<TEntity> Set<TEntity>() where TEntity : Entity
    {
      var entityType = typeof(TEntity);

      if (entityType == typeof(Airhostess))
      {
        return (IRepository<TEntity>)AirhostessRepository;
      }
      else if (entityType == typeof(Crew))
      {
        return (IRepository<TEntity>)CrewRepository;
      }
      else if (entityType == typeof(Departure))
      {
        return (IRepository<TEntity>)DepartureRepository;
      }
      else if (entityType == typeof(Flight))
      {
        return (IRepository<TEntity>)FlightRepository;
      }
      else if (entityType == typeof(Pilot))
      {
        return (IRepository<TEntity>)PilotRepository;
      }
      else if (entityType == typeof(Plane))
      {
        return (IRepository<TEntity>)PlaneRepository;
      }
      else if (entityType == typeof(PlaneType))
      {
        return (IRepository<TEntity>)PlaneTypeRepository;
      }
      else if (entityType == typeof(Ticket))
      {
        return (IRepository<TEntity>)TicketRepository;
      }

      throw new NotImplementedException($"No repository for: {entityType.Name}");
    }

    public void Dispose()
    {
      _dbContext.Dispose();
    }
  }
}