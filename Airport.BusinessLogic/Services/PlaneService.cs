using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using FluentValidation;

using Airport.Common.Exceptions;
using Airport.Common.DTOs;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;
using System.Threading.Tasks;

namespace Airport.BusinessLogic.Services
{
  public interface IPlaneService : IService<PlaneDTO>
  {
    Task<IList<PlaneDetailsDTO>> GetAllDetailsAsync();
    Task<PlaneDetailsDTO> GetDetailsAsync(int id);
  }

  public class PlaneService : BaseService<PlaneDTO, Plane>, IPlaneService
  {
    public PlaneService(IUnitOfWork unitOfWork, IValidator<PlaneDTO> planeDTOValidator)
      : base(unitOfWork, planeDTOValidator)
    { }

    public async Task<IList<PlaneDetailsDTO>> GetAllDetailsAsync()
    {
      var planes = await _unitOfWork.Set<Plane>().DetailsAsync();
      return await planes.ToAsyncEnumerable().Select(PlaneDetailsDTO.Create).ToList();
    }

    public async Task<PlaneDetailsDTO> GetDetailsAsync(int id)
    {
      var plane = await _unitOfWork.Set<Plane>()
        .DetailsAsync(id);

      if (plane == null)
        throw new NotFoundException("Plane with such id was not found");

      return PlaneDetailsDTO.Create(plane);
    }
  }
}