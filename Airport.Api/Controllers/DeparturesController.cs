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
  public class DeparturesController : Controller
  {
    IDepartureService _departureService;
    IValidator<DepartureDTO> _depatureModelValidator;

    public DeparturesController(
      IDepartureService departureService,
      IValidator<DepartureDTO> depatureModelValidator
    )
    {
      _departureService = departureService;
      _depatureModelValidator = depatureModelValidator;
    }

    // GET api/airhostesses
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var entites = await _departureService.GetAllAsync();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _departureService.GetByIdAsync(id);
      return Json(entites);
    }

    [HttpGet("details")]
    public async Task<IActionResult> GetDetails()
    {
      var entites = await _departureService.GetAllDetailsAsync();
      return Json(entites);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id)
    {
      var entites = await _departureService.GetDetailsAsync(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]DepartureDTO value)
    {
      var validationResult = await _depatureModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = await _departureService.CreateAsync(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]DepartureDTO value)
    {
      var validationResult = await _depatureModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = await _departureService.UpdateAsync(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _departureService.DeleteAsync(id);
    }
  }
}
