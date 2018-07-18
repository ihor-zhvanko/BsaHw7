using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Airport.Data.Repositories
{
  public interface IRepository<TEntity>
  {
    Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null);

    Task<TEntity> CreateAsync(TEntity entity);

    TEntity Update(TEntity entity);

    void Delete(TEntity entity);

    Task<TEntity> GetAsync(int id);
    Task DeleteAsync(int id);
    Task<IList<TEntity>> DetailsAsync(Expression<Func<TEntity, bool>> filter = null);
    Task<TEntity> DetailsAsync(int id);
  }
}