using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Airport.Data.Repositories
{
  public interface IRepository<TEntity>
  {
    Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null);

    Task<TEntity> CreateAsync(TEntity entity);

    TEntity Update(TEntity entity);

    void Delete(TEntity entity);

    Task<TEntity> Get(int id);
    Task DeleteAsync(int id);
    Task<IList<TEntity>> Details(Expression<Func<TEntity, bool>> filter = null);
  }
}