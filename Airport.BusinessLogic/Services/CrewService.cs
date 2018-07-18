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

using Airport.MockApi;

namespace Airport.BusinessLogic.Services
{
  public interface ICrewService : IService<CrewDTO>
  {
    Task<IList<CrewDetailsDTO>> GetAllDetailsAsync();
    Task<CrewDetailsDTO> GetDetailsAsync(int id);
    Task<IList<CrewDetailsDTO>> LoadCrewsFromMockApi();
  }

  public class CrewService : BaseService<CrewDTO, Crew>, ICrewService
  {
    IMockApiConnector _mockApiConnector;

    public CrewService(
      IUnitOfWork unitOfWork,
      IValidator<CrewDTO> crewDTOValidator,
      IMockApiConnector mockApiConnector
    )
      : base(unitOfWork, crewDTOValidator)
    {
      _mockApiConnector = mockApiConnector;
    }

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

    public async Task<IList<CrewDetailsDTO>> LoadCrewsFromMockApi()
    {
      //TODO: I will refactor this shit later. Reason: Deadline 
      var crewResponse = await _mockApiConnector.GetCrews();
      var newCrews = crewResponse.Select(x =>
      {
        var pilot = x.Pilot.First();
        var newCrew = new Crew
        {
          Pilot = new Pilot
          {
            FirstName = pilot.FirstName,
            LastName = pilot.LastName,
            BirthDate = pilot.BirthDate,
            Experience = pilot.Exp
          },
          Airhostesses = x.Stewardess.Select(y => new Airhostess
          {
            FirstName = y.FirstName,
            LastName = y.LastName,
            BirthDate = y.BirthDate
          }).ToList()
        };

        return newCrew;
      }).ToList();
      var created = await _unitOfWork.CrewRepository.CreateManyAsync(newCrews);

      var saveChangesTask = _unitOfWork.SaveChangesAsync();

      var newCrewsCsvLines = crewResponse.Select(x =>
        $"{x.Id},{x.Pilot.First().FirstName},{x.Pilot.First().LastName},{x.Stewardess.Count}"
      );

      var currentDate = DateTime.Now.ToString("dd.MM.yyyy");
      var currentTime = DateTime.Now.ToString("H.mm.ss");
      var writeFileTask = File.WriteAllLinesAsync($"log_{currentDate}_{currentTime}.txt", newCrewsCsvLines);

      await Task.WhenAll(new[] { saveChangesTask, writeFileTask });

      return await created.ToAsyncEnumerable().Select(CrewDetailsDTO.Create).ToList();
    }
  }
}