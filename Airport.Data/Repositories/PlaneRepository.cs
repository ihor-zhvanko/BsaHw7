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
  public interface IPlaneRepository : IRepository<Plane>
  {

  }

  public class PlaneRepository : Repository<Plane>, IPlaneRepository
  {
    private AirportDbContext _dbContext;
    public PlaneRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override async Task<IList<Plane>> Details(Expression<Func<Plane, bool>> filter = null)
    {
      var planes = _dbContext.Plane
        .Include(x => x.PlaneType);
      if (filter != null)
        return await planes.Where(filter).ToListAsync();

      return await planes.ToListAsync();
    }
  }
}