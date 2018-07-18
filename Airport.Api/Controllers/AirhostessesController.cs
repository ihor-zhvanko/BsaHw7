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
    public async Task<IActionResult> Get()
    {
      var entites = await _airhostessesService.GetAllAsync();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _airhostessesService.GetByIdAsync(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]AirhostessDTO value)
    {
      var validationResult = await _airhostessValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = await _airhostessesService.CreateAsync(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]AirhostessDTO value)
    {
      var validationResult = await _airhostessValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = await _airhostessesService.UpdateAsync(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _airhostessesService.DeleteAsync(id);
    }
  }
}
