using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using FluentValidation;

using Airport.Common.Exceptions;
using Airport.Common.DTOs;

using Airport.BusinessLogic.Services;

namespace Airport.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class FlightsController : Controller
  {
    IFlightService _flightService;
    IValidator<FlightDTO> _flightModelValidator;

    public FlightsController(
      IFlightService flightService,
      IValidator<FlightDTO> flightModelValidator
    )
    {
      _flightService = flightService;
      _flightModelValidator = flightModelValidator;
    }

    // GET api/airhostesses
    [HttpGet]
    public IActionResult Get()
    {
      var entites = _flightService.GetAll();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _flightService.GetById(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]FlightDTO value)
    {
      var validationResult = _flightModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _flightService.Create(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]FlightDTO value)
    {
      var validationResult = _flightModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _flightService.Update(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _flightService.Delete(id);
    }
  }
}
