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
  public class PlaneTypesController : Controller
  {
    IPlaneTypeService _planeTypeService;
    IValidator<PlaneTypeDTO> _planeTypeModelValidator;

    public PlaneTypesController(
      IPlaneTypeService planeTypeService,
      IValidator<PlaneTypeDTO> planeTypeModelValidator
    )
    {
      _planeTypeService = planeTypeService;
      _planeTypeModelValidator = planeTypeModelValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var entites = await _planeTypeService.GetAllAsync();
      return Json(entites);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _planeTypeService.GetByIdAsync(id);
      return Json(entites);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]PlaneTypeDTO value)
    {
      var validationResult = await _planeTypeModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = await _planeTypeService.CreateAsync(value);
      return Json(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]PlaneTypeDTO value)
    {
      var validationResult = await _planeTypeModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = await _planeTypeService.UpdateAsync(value);

      return Json(entity);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _planeTypeService.DeleteAsync(id);
    }
  }
}
