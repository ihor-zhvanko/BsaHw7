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
    public IActionResult Get()
    {
      var entites = _departureService.GetAll();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _departureService.GetById(id);
      return Json(entites);
    }

    [HttpGet("details")]
    public IActionResult GetDetails()
    {
      var entites = _departureService.GetAllDetails();
      return Json(entites);
    }

    [HttpGet("{id}/details")]
    public IActionResult GetDetails(int id)
    {
      var entites = _departureService.GetDetails(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]DepartureDTO value)
    {
      var validationResult = _depatureModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _departureService.Create(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]DepartureDTO value)
    {
      var validationResult = _depatureModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _departureService.Update(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _departureService.Delete(id);
    }
  }
}
