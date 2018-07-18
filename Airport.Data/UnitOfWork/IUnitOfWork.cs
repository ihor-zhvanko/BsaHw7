using System;
using System.Threading.Tasks;
using Airport.Data.Repositories;
using Airport.Data.Models;

namespace Airport.Data.UnitOfWork
{
  public interface IUnitOfWork : IDisposable
  {
    IAirhostessRepository AirhostessRepository { get; }
    ICrewRepository CrewRepository { get; }
    IDepartureRepository DepartureRepository { get; }
    IFlightRepository FlightRepository { get; }
    IPilotRepository PilotRepository { get; }
    IPlaneRepository PlaneRepository { get; }
    IPlaneTypeRepository PlaneTypeRepository { get; }
    ITicketRepository TicketRepository { get; }
    IRepository<TEntity> Set<TEntity>() where TEntity : Entity;
    int SaveChanges();
    Task<int> SaveChangesAsync();
  }
}