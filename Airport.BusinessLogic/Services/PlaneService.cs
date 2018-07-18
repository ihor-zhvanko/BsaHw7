using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using FluentValidation;

using Airport.Common.Exceptions;
using Airport.Common.DTOs;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;

namespace Airport.BusinessLogic.Services
{
  public interface IPlaneService : IService<PlaneDTO>
  {
    IList<PlaneDetailsDTO> GetAllDetails();
    PlaneDetailsDTO GetDetails(int id);
  }

  public class PlaneService : BaseService<PlaneDTO, Plane>, IPlaneService
  {
    public PlaneService(IUnitOfWork unitOfWork, IValidator<PlaneDTO> planeDTOValidator)
      : base(unitOfWork, planeDTOValidator)
    { }

    public IList<PlaneDetailsDTO> GetAllDetails()
    {
      var planes = _unitOfWork.Set<Plane>().Details();
      return planes.Select(PlaneDetailsDTO.Create).ToList();
    }

    public PlaneDetailsDTO GetDetails(int id)
    {
      var plane = _unitOfWork.Set<Plane>()
        .Details(x => x.Id == id).FirstOrDefault();

      if (plane == null)
        throw new NotFoundException("Plane with such id was not found");

      return PlaneDetailsDTO.Create(plane);
    }
  }
}