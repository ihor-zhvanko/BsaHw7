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
    public IActionResult Get()
    {
      var entites = _planeTypeService.GetAll();
      return Json(entites);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _planeTypeService.GetById(id);
      return Json(entites);
    }

    [HttpPost]
    public IActionResult Post([FromBody]PlaneTypeDTO value)
    {
      var validationResult = _planeTypeModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _planeTypeService.Create(value);
      return Json(entity);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]PlaneTypeDTO value)
    {
      var validationResult = _planeTypeModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _planeTypeService.Update(value);

      return Json(entity);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _planeTypeService.Delete(id);
    }
  }
}
