using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using FluentValidation;

using Airport.Common.DTOs;
using Airport.Common.Exceptions;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;

namespace Airport.BusinessLogic.Services
{
  public interface IDepartureService : IService<DepartureDTO>
  {
    IList<DepartureDetailsDTO> GetAllDetails();
    DepartureDetailsDTO GetDetails(int id);
    IList<DepartureDTO> GetByCrewId(int crewId);
  }

  public class DepartureService : BaseService<DepartureDTO, Departure>, IDepartureService
  {
    public DepartureService(
      IUnitOfWork unitOfWork,
      IValidator<DepartureDTO> departureDTOValidator
    ) : base(unitOfWork, departureDTOValidator)
    { }

    public IList<DepartureDetailsDTO> GetAllDetails()
    {
      var departures = _unitOfWork.Set<Departure>().Details();

      var crews = _unitOfWork.Set<Crew>()
        .Details(x => departures.Any(y => x.Id == y.CrewId))
        .Select(CrewDetailsDTO.Create);

      var planes = _unitOfWork.Set<Plane>()
        .Details(x => departures.Any(y => x.Id == y.PlaneId))
        .Select(PlaneDetailsDTO.Create);

      return departures.Select(x =>
      {
        var plane = planes.First(p => p.Id == x.PlaneId);
        var crew = crews.First(c => c.Id == x.CrewId);
        return DepartureDetailsDTO.Create(x, plane, crew);
      }).ToList();
    }

    public DepartureDetailsDTO GetDetails(int id)
    {
      var departure = _unitOfWork.Set<Departure>()
        .Details(x => x.Id == id).FirstOrDefault();

      if (departure == null)
        throw new NotFoundException("Departure with such id was not found");

      var crew = _unitOfWork.Set<Crew>()
        .Details(x => x.Id == departure.CrewId)
        .Select(CrewDetailsDTO.Create).First();
      var plane = _unitOfWork.Set<Plane>()
        .Details(x => x.Id == departure.PlaneId)
        .Select(PlaneDetailsDTO.Create).First();

      return DepartureDetailsDTO.Create(departure, plane, crew);
    }

    public IList<DepartureDTO> GetByCrewId(int crewId)
    {
      var departures = _unitOfWork.Set<Departure>().Get(x => x.CrewId == crewId).ToList();
      return Mapper.Map<IList<DepartureDTO>>(departures);
    }
  }
}
