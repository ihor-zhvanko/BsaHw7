using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Airport.Data.MockData;
using Airport.Data.Models;
using System.Threading.Tasks;

namespace Airport.Data.Repositories
{
  public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
  {
    private DbContext _dbContext;
    public Repository(DbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null)
    {
      if (filter != null)
        return await _dbContext.Set<TEntity>().Where(filter).ToListAsync();

      return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
      await _dbContext.Set<TEntity>().AddAsync(entity);
      return entity;
    }

    public virtual async Task<IList<TEntity>> CreateManyAsync(IList<TEntity> entity)
    {
      await _dbContext.Set<TEntity>().AddRangeAsync(entity);
      return entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
      _dbContext.Update(entity);
      return entity;
    }

    public virtual void Delete(TEntity entity)
    {
      _dbContext.Set<TEntity>().Remove(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
      var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
      if (entity != null)
        _dbContext.Remove(entity);
    }

    public virtual async Task<TEntity> GetAsync(int id)
    {
      var result = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
      return result;
    }

    public abstract Task<IList<TEntity>> DetailsAsync(Expression<Func<TEntity, bool>> filter = null);

    public abstract Task<TEntity> DetailsAsync(int id);
  }
}