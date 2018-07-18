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

    public override IEnumerable<Airhostess> Details(Expression<Func<Airhostess, bool>> filter = null)
    {
      var airhostesses = _dbContext.Airhostess.Include(x => x.Crew);
      if (filter != null)
        return airhostesses.Where(filter);

      return airhostesses;
    }
  }
}