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
  public interface IPlaneTypeRepository : IRepository<PlaneType>
  {

  }

  public class PlaneTypeRepository : Repository<PlaneType>, IPlaneTypeRepository
  {
    private AirportDbContext _dbContext;
    public PlaneTypeRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override async Task<IList<PlaneType>> Details(Expression<Func<PlaneType, bool>> filter = null)
    {
      // could be implemented if needed
      var planeTypes = _dbContext.PlaneType;
      if (filter != null)
        return await planeTypes.Where(filter).ToListAsync();

      return await planeTypes.ToListAsync();
    }
  }
}