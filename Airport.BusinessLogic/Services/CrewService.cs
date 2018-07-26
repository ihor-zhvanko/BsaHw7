using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using FluentValidation;

using Airport.Common.DTOs;
using Airport.Common.Exceptions;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;


namespace Airport.BusinessLogic.Services
{
  public interface ICrewService : IService<CrewDTO>
  {
    Task<IList<CrewDetailsDTO>> GetAllDetailsAsync();
    Task<CrewDetailsDTO> GetDetailsAsync(int id);
  }

  public class CrewService : BaseService<CrewDTO, Crew>, ICrewService
  {

    public CrewService(
      IUnitOfWork unitOfWork,
      IValidator<CrewDTO> crewDTOValidator
    )
      : base(unitOfWork, crewDTOValidator)
    { }

    public async Task<IList<CrewDetailsDTO>> GetAllDetailsAsync()
    {
      var crews = await _unitOfWork.Set<Crew>().DetailsAsync();
      //AsyncEnumerable.ToAsyncEnumerable()
      return await crews.ToAsyncEnumerable().Select(CrewDetailsDTO.Create).ToList();
    }

    public async Task<CrewDetailsDTO> GetDetailsAsync(int id)
    {
      var crew = await _unitOfWork.Set<Crew>()
        .DetailsAsync(id);

      if (crew == null)
      {
        throw new NotFoundException("Crew with such id was not found");
      }
      return CrewDetailsDTO.Create(crew);
    }
  }
}