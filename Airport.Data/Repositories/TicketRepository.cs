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
  public interface ITicketRepository : IRepository<Ticket>
  {

  }

  public class TicketRepository : Repository<Ticket>, ITicketRepository
  {
    private AirportDbContext _dbContext;
    public TicketRepository(AirportDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public override async Task<IList<Ticket>> DetailsAsync(Expression<Func<Ticket, bool>> filter = null)
    {
      var tickets = _dbContext.Ticket.Include(x => x.Flight);
      if (filter != null)
        return await tickets.Where(filter).ToListAsync();

      return await tickets.ToListAsync();
    }

    public override async Task<Ticket> DetailsAsync(int id)
    {
      var tickets = _dbContext.Ticket.Include(x => x.Flight);
      return await tickets.FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}