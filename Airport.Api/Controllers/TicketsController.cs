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
    public async Task<IActionResult> Get()
    {
      var entites = await _ticketService.GetAllAsync();
      return Json(entites);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var entites = await _ticketService.GetByIdAsync(id);
      return Json(entites);
    }

    [HttpGet("details")]
    public async Task<IActionResult> GetDetails()
    {
      var entites = await _ticketService.GetAllDetailsAsync();
      return Json(entites);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id)
    {
      var entites = await _ticketService.GetDetailsAsync(id);
      return Json(entites);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]TicketDTO value)
    {
      var validationResult = await _ticketModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      var entity = await _ticketService.CreateAsync(value);
      return Json(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]TicketDTO value)
    {
      var validationResult = await _ticketModelValidator.ValidateAsync(value);
      if (!validationResult.IsValid)
        throw new BadRequestException(validationResult.Errors);

      value.Id = id;
      var entity = await _ticketService.UpdateAsync(value);

      return Json(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
      await _ticketService.DeleteAsync(id);
    }
  }
}
