using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using FluentValidation;

using Airport.Common.Exceptions;
using Airport.Common.DTOs;
using Airport.Common.InputModels;

using Airport.BusinessLogic.Services;

namespace Airport.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class CrewsController : Controller
  {
    ICrewService _crewService;
    IAirhostessService _airhostessesService;
    IDepartureService _departureService;
    IValidator<CrewInputModel> _crewInputModelValidator;

    public CrewsController(
      ICrewService crewService,
      IAirhostessService airhostessesService,
      IDepartureService departureService,
      IValidator<CrewInputModel> crewInputModelValidator
    )
    {
      _airhostessesService = airhostessesService;
      _crewService = crewService;
      _departureService = departureService;
      _crewInputModelValidator = crewInputModelValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var entites = await _crewService.GetAllDetailsAsync();
      return Json(entites);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _crewService.GetDetailsAsync(id);
      return Json(entites);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CrewInputModel value)
    {
      var validationResult = await _crewInputModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var createdCrew = await _crewService.CreateAsync(value);
      await _airhostessesService.AssignToCrewAsync(value.AirhostessIds, createdCrew.Id);

      var details = await _crewService.GetDetailsAsync(createdCrew.Id);
      return Json(details);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]CrewInputModel value)
    {
      var validationResult = await _crewInputModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;

      await _crewService.UpdateAsync(value);
      await _airhostessesService.AssignToCrewAsync(value.AirhostessIds, id);

      var details = await _crewService.GetDetailsAsync(id);
      return Json(details);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _crewService.DeleteAsync(id);
    }

    [HttpGet("{id}/departures")]
    public async Task<IActionResult> GetCrewDepartures(int id)
    {
      var departures = await _departureService.GetByCrewIdAsync(id);
      return Json(departures);
    }

    [HttpGet("load")]
    public async Task<IActionResult> LoadCrews()
    {
      var syncedCrews = await _crewService.LoadCrewsFromMockApi();

      return Json(syncedCrews);
    }
  }
}
