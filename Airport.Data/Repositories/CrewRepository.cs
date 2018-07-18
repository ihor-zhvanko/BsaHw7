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
  public interface ICrewRepository : IRepository<Crew>
  {

  }

  public class CrewRepository : Repository<Crew>, ICrewRepository
  {
    private AirportDbContext _dbContext;
    public CrewRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override IEnumerable<Crew> Details(Expression<Func<Crew, bool>> filter = null)
    {
      var crews = _dbContext.Crew.Include(x => x.Airhostesses).Include(x => x.Pilot);
      if (filter != null)
        return crews.Where(filter);

      return crews;
    }
  }
}