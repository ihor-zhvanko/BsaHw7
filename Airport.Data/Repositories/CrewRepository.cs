using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Airport.Data.MockData;
using Airport.Data.Models;
using Airport.Data.DatabaseContext;
using System.Threading.Tasks;

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

    public override async Task<IList<Crew>> DetailsAsync(Expression<Func<Crew, bool>> filter = null)
    {
      var crews = _dbContext.Crew.Include(x => x.Airhostesses).Include(x => x.Pilot);
      if (filter != null)
        return await crews.Where(filter).ToListAsync();

      return await crews.ToListAsync();
    }

    public override async Task<Crew> DetailsAsync(int id)
    {
      var crews = _dbContext.Crew.Include(x => x.Airhostesses).Include(x => x.Pilot);
      return await crews.FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}