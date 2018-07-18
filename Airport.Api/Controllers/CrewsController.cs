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
    public IActionResult Get()
    {
      var entites = _crewService.GetAllDetails();
      return Json(entites);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _crewService.GetDetails(id);
      return Json(entites);
    }

    [HttpPost]
    public IActionResult Post([FromBody]CrewInputModel value)
    {
      var validationResult = _crewInputModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var newId = _crewService.Create(value).Id;
      _airhostessesService.AssignToCrew(value.AirhostessIds, newId);

      var details = _crewService.GetDetails(newId);
      return Json(details);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]CrewInputModel value)
    {
      var validationResult = _crewInputModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;

      _crewService.Update(value);
      _airhostessesService.AssignToCrew(value.AirhostessIds, id);

      var details = _crewService.GetDetails(id);
      return Json(details);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _crewService.Delete(id);
    }

    [HttpGet("{id}/departures")]
    public IActionResult GetCrewDepartures(int id)
    {
      var departures = _departureService.GetByCrewId(id);
      return Json(departures);
    }
  }
}
