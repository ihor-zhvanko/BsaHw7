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
  public class TicketsController : Controller
  {
    ITicketService _ticketService;
    IValidator<TicketDTO> _ticketModelValidator;

    public TicketsController(
      ITicketService ticketService,
      IValidator<TicketDTO> ticketModelValidator
    )
    {
      _ticketService = ticketService;
      _ticketModelValidator = ticketModelValidator;
    }

    // GET api/airhostesses
    [HttpGet]
    public IActionResult Get()
    {
      var entites = _ticketService.GetAll();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var entites = _ticketService.GetById(id);
      return Json(entites);
    }

    [HttpGet("details")]
    public IActionResult GetDetails()
    {
      var entites = _ticketService.GetAllDetails();
      return Json(entites);
    }

    [HttpGet("{id}/details")]
    public IActionResult GetDetails(int id)
    {
      var entites = _ticketService.GetDetails(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]TicketDTO value)
    {
      var validationResult = _ticketModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = _ticketService.Create(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]TicketDTO value)
    {
      var validationResult = _ticketModelValidator.Validate(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = _ticketService.Update(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _ticketService.Delete(id);
    }
  }
}
