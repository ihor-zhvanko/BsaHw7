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
  public class PilotsController : Controller
  {
    IPilotService _pilotService;
    IValidator<PilotDTO> _pilotModelValidator;

    public PilotsController(
      IPilotService pilotService,
      IValidator<PilotDTO> pilotModelValidator
    )
    {
      _pilotService = pilotService;
      _pilotModelValidator = pilotModelValidator;
    }

    // GET api/airhostesses
    [HttpGet]
    public IActionResult Get()
    {
      var entites = _pilotService.GetAll();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _pilotService.GetById(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]PilotDTO value)
    {
      var validationResult = _pilotModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _pilotService.Create(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]PilotDTO value)
    {
      var validationResult = _pilotModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _pilotService.Update(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _pilotService.Delete(id);
    }
  }
}
