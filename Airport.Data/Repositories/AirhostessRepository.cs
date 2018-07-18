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
  public interface IAirhostessRepository : IRepository<Airhostess>
  {
  }

  public class AirhostessRepository : Repository<Airhostess>, IAirhostessRepository
  {
    private AirportDbContext _dbContext;
    public AirhostessRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override async Task<IList<Airhostess>> DetailsAsync(Expression<Func<Airhostess, bool>> filter = null)
    {
      var airhostesses = _dbContext.Airhostess.Include(x => x.Crew);
      if (filter != null)
        return await airhostesses.Where(filter).ToListAsync();

      return await airhostesses.ToListAsync();
    }

    public override async Task<Airhostess> DetailsAsync(int id)
    {
      var airhostesses = _dbContext.Airhostess.Include(x => x.Crew);
      return await airhostesses.FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}