using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Airport.Data.MockData;
using Airport.Data.Models;
using Airport.Data.DatabaseContext;

namespace Airport.Data.Repositories
{
  public interface IFlightRepository : IRepository<Flight>
  {

  }

  public class FlightRepository : Repository<Flight>, IFlightRepository
  {
    private AirportDbContext _dbContext;
    public FlightRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override IEnumerable<Flight> Details(Expression<Func<Flight, bool>> filter = null)
    {
      // could be implemented if needed
      return _dbContext.Flight;
    }
  }
}