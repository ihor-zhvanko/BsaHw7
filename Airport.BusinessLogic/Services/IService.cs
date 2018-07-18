using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airport.BusinessLogic.Services
{
  public interface IService<TDTO>
  {
    Task<IList<TDTO>> GetAllAsync();
    Task<TDTO> GetByIdAsync(int id);
    Task<TDTO> CreateAsync(TDTO model);
    Task<TDTO> UpdateAsync(TDTO model);
    Task DeleteAsync(TDTO model);
    Task DeleteAsync(int id);
  }
}