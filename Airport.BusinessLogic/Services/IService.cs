using System;
using System.Collections.Generic;

namespace Airport.BusinessLogic.Services
{
  public interface IService<TDTO>
  {
    IList<TDTO> GetAll();
    TDTO GetById(int id);
    TDTO Create(TDTO model);
    TDTO Update(TDTO model);
    void Delete(TDTO model);
    void Delete(int id);
  }
}