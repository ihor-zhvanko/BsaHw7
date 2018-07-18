using System;
using System.Linq;
using System.Collections.Generic;

using Airport.Common.DTOs;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;

using AutoMapper;
using FluentValidation;
using System.Threading.Tasks;

namespace Airport.BusinessLogic.Services
{
  public interface IAirhostessService : IService<AirhostessDTO>
  {
    Task<IList<AirhostessDTO>> GetByCrewIdAsync(int crewId);
    Task AssignToCrewAsync(IList<int> airhostessIds, int crewId);
  }

  public class AirhostessService : BaseService<AirhostessDTO, Airhostess>, IAirhostessService
  {
    public AirhostessService(IUnitOfWork unitOfWork, IValidator<AirhostessDTO> airhostessDTOValidator)
      : base(unitOfWork, airhostessDTOValidator)
    { }

    public async Task<IList<AirhostessDTO>> GetByCrewIdAsync(int crewId)
    {
      var airhostesses = await _unitOfWork.Set<Airhostess>().GetAsync((x) => x.CrewId == crewId);

      return Mapper.Map<IList<AirhostessDTO>>(airhostesses);
    }

    public async Task AssignToCrewAsync(IList<int> airhostessIds, int crewId)
    {
      var toUpdate = await _unitOfWork.Set<Airhostess>()
        .GetAsync(x => airhostessIds.Any(y => x.Id == y) || x.CrewId == crewId);

      foreach (var item in toUpdate)
      {
        if (await airhostessIds.ToAsyncEnumerable().Any(x => x == item.Id))
          item.CrewId = crewId;
        else
          item.CrewId = null;

        _unitOfWork.Set<Airhostess>().Update(item);
        await _unitOfWork.SaveChangesAsync();
      }
    }
  }
}