using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using FluentValidation;

using Airport.Common.DTOs;
using Airport.Common.Exceptions;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;
using System.Threading.Tasks;

namespace Airport.BusinessLogic.Services
{
  public interface IDepartureService : IService<DepartureDTO>
  {
    Task<IList<DepartureDetailsDTO>> GetAllDetailsAsync();
    Task<DepartureDetailsDTO> GetDetailsAsync(int id);
    Task<IList<DepartureDTO>> GetByCrewIdAsync(int crewId);
  }

  public class DepartureService : BaseService<DepartureDTO, Departure>, IDepartureService
  {
    public DepartureService(
      IUnitOfWork unitOfWork,
      IValidator<DepartureDTO> departureDTOValidator
    ) : base(unitOfWork, departureDTOValidator)
    { }

    public async Task<IList<DepartureDetailsDTO>> GetAllDetailsAsync()
    {
      var departures = await _unitOfWork.Set<Departure>().DetailsAsync();

      var crews = await _unitOfWork.Set<Crew>()
        .DetailsAsync(x => departures.Any(y => x.Id == y.CrewId));

      var planes = await _unitOfWork.Set<Plane>()
        .DetailsAsync(x => departures.Any(y => x.Id == y.PlaneId));

      return await departures.ToAsyncEnumerable().Select(x =>
      {
        var plane = planes.First(p => p.Id == x.PlaneId);
        var crew = crews.First(c => c.Id == x.CrewId);
        return DepartureDetailsDTO.Create(x, PlaneDetailsDTO.Create(plane), CrewDetailsDTO.Create(crew));
      }).ToList();
    }

    public async Task<DepartureDetailsDTO> GetDetailsAsync(int id)
    {
      var departure = await _unitOfWork.Set<Departure>()
        .DetailsAsync(id);

      if (departure == null)
        throw new NotFoundException("Departure with such id was not found");

      var crew = await _unitOfWork.Set<Crew>()
        .DetailsAsync(departure.CrewId);
      var plane = await _unitOfWork.Set<Plane>()
        .DetailsAsync(departure.PlaneId);

      return DepartureDetailsDTO.Create(departure, PlaneDetailsDTO.Create(plane), CrewDetailsDTO.Create(crew));
    }

    public async Task<IList<DepartureDTO>> GetByCrewIdAsync(int crewId)
    {
      var departures = await _unitOfWork.Set<Departure>().GetAsync(x => x.CrewId == crewId);
      return Mapper.Map<IList<DepartureDTO>>(departures);
    }
  }
}
