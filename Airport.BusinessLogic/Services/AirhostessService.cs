using System;
using System.Linq;
using System.Collections.Generic;

using Airport.Common.DTOs;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;

using AutoMapper;
using FluentValidation;

namespace Airport.BusinessLogic.Services
{
  public interface IAirhostessService : IService<AirhostessDTO>
  {
    IList<AirhostessDTO> GetByCrewId(int crewId);
    void AssignToCrew(IList<int> airhostessIds, int crewId);
  }

  public class AirhostessService : BaseService<AirhostessDTO, Airhostess>, IAirhostessService
  {
    public AirhostessService(IUnitOfWork unitOfWork, IValidator<AirhostessDTO> airhostessDTOValidator)
      : base(unitOfWork, airhostessDTOValidator)
    { }

    public IList<AirhostessDTO> GetByCrewId(int crewId)
    {
      var airhostesses = _unitOfWork.Set<Airhostess>().Get((x) => x.CrewId == crewId);

      return Mapper.Map<IList<AirhostessDTO>>(airhostesses);
    }

    public void AssignToCrew(IList<int> airhostessIds, int crewId)
    {
      var toUpdate = _unitOfWork.Set<Airhostess>()
        .Get(x => airhostessIds.Any(y => x.Id == y) || x.CrewId == crewId).ToList();
      foreach (var item in toUpdate)
      {
        if (airhostessIds.Any(x => x == item.Id))
          item.CrewId = crewId;
        else
          item.CrewId = null;

        _unitOfWork.Set<Airhostess>().Update(item);
        _unitOfWork.SaveChanges();
      }
    }
  }
}