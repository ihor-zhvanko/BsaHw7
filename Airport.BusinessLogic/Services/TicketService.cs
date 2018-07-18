using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;

using Airport.Common.Exceptions;
using Airport.Common.DTOs;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;
using System.Threading.Tasks;

namespace Airport.BusinessLogic.Services
{
  public interface ITicketService : IService<TicketDTO>
  {
    IList<TicketDetailsDTO> GetAllDetails();
    TicketDetailsDTO GetDetails(int id);
  }

  public class TicketService : BaseService<TicketDTO, Ticket>, ITicketService
  {
    public TicketService(
      IUnitOfWork unitOfWork, IValidator<TicketDTO> ticketDTOValidator
    ) : base(unitOfWork, ticketDTOValidator)
    { }

    public async Task<IList<TicketDetailsDTO>> GetAllDetailsAsync()
    {
      var tickets = await _unitOfWork.Set<Ticket>().DetailsAsync();
      return await tickets.ToAsyncEnumerable().Select(TicketDetailsDTO.Create).ToList();
    }

    public async Task<TicketDetailsDTO> GetDetailsAsync(int id)
    {
      var ticket = await _unitOfWork.Set<Ticket>()
        .DetailsAsync(id);

      if (ticket == null)
        throw new NotFoundException("Ticket with such id was not found");

      return TicketDetailsDTO.Create(ticket);
    }
  }
}