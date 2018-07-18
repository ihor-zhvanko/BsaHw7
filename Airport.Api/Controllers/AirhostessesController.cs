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
  public class AirhostessesController : Controller
  {
    IAirhostessService _airhostessesService;
    IValidator<AirhostessDTO> _airhostessValidator;

    public AirhostessesController(
      IAirhostessService airhostessesService,
      IValidator<AirhostessDTO> airhostessValidator
    )
    {
      _airhostessesService = airhostessesService;
      _airhostessValidator = airhostessValidator;
    }

    // GET api/airhostesses
    [HttpGet]
    public IActionResult Get()
    {
      var entites = _airhostessesService.GetAll();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _airhostessesService.GetById(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]AirhostessDTO value)
    {
      var validationResult = _airhostessValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _airhostessesService.Create(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]AirhostessDTO value)
    {
      var validationResult = _airhostessValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _airhostessesService.Update(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _airhostessesService.Delete(id);
    }
  }
}
