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
    public IActionResult Get()
    {
      var entites = _planeService.GetAll();
      return Json(entites);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _planeService.GetById(id);
      return Json(entites);
    }

    [HttpGet("details")]
    public IActionResult GetDetails()
    {
      var entites = _planeService.GetAllDetails();
      return Json(entites);
    }

    [HttpGet("{id}/details")]
    public IActionResult GetDetails(int id)
    {
      var entites = _planeService.GetDetails(id);
      return Json(entites);
    }

    [HttpPost]
    public IActionResult Post([FromBody]PlaneDTO value)
    {
      var validationResult = _planeModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _planeService.Create(value);
      return Json(entity);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]PlaneDTO value)
    {
      var validationResult = _planeModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _planeService.Update(value);

      return Json(entity);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _planeService.Delete(id);
    }
  }
}
