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
    public async Task<IActionResult> Get()
    {
      var entites = await _flightService.GetAllAsync();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _flightService.GetByIdAsync(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]FlightDTO value)
    {
      var validationResult = await _flightModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = await _flightService.CreateAsync(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]FlightDTO value)
    {
      var validationResult = await _flightModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = await _flightService.UpdateAsync(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _flightService.DeleteAsync(id);
    }
  }
}
