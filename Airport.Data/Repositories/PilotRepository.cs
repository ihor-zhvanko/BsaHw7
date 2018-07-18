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
  public interface IPilotRepository : IRepository<Pilot>
  {

  }

  public class PilotRepository : Repository<Pilot>, IPilotRepository
  {
    private AirportDbContext _dbContext;
    public PilotRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override IEnumerable<Pilot> Details(Expression<Func<Pilot, bool>> filter = null)
    {
      // could be implemented if needed
      if (filter != null)
        return _dbContext.Pilot.Where(filter);

      return _dbContext.Pilot;
    }
  }
}