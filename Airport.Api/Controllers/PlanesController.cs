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
  public class PlanesController : Controller
  {
    IPlaneService _planeService;
    IValidator<PlaneDTO> _planeModelValidator;

    public PlanesController(
      IPlaneService planeService,
      IValidator<PlaneDTO> planeModelValidator
    )
    {
      _planeService = planeService;
      _planeModelValidator = planeModelValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var entites = await _planeService.GetAllAsync();
      return Json(entites);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _planeService.GetByIdAsync(id);
      return Json(entites);
    }

    [HttpGet("details")]
    public async Task<IActionResult> GetDetails()
    {
      var entites = await _planeService.GetAllDetailsAsync();
      return Json(entites);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id)
    {
      var entites = await _planeService.GetDetailsAsync(id);
      return Json(entites);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]PlaneDTO value)
    {
      var validationResult = await _planeModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = await _planeService.CreateAsync(value);
      return Json(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]PlaneDTO value)
    {
      var validationResult = await _planeModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = await _planeService.UpdateAsync(value);

      return Json(entity);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _planeService.DeleteAsync(id);
    }
  }
}
