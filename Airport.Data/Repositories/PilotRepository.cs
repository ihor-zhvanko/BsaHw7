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

    public override async Task<IList<Pilot>> DetailsAsync(Expression<Func<Pilot, bool>> filter = null)
    {
      // could be implemented if needed
      if (filter != null)
        return await _dbContext.Pilot.Where(filter).ToListAsync();

      return await _dbContext.Pilot.ToListAsync();
    }

    public override async Task<Pilot> DetailsAsync(int id)
    {
      return await _dbContext.Pilot.FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}