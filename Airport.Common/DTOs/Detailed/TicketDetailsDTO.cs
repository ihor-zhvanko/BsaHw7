using System;
using AutoMapper;

using Airport.Data.Models;

namespace Airport.Common.DTOs
{
  public class TicketDetailsDTO
  {
    public int Id { get; set; }
    public double Price { get; set; }
    public FlightDTO Flight { get; set; }

    public static TicketDetailsDTO Create(Ticket ticket)
    {
      return new TicketDetailsDTO
      {
        Id = ticket.Id,
        Price = ticket.Price,
        Flight = Mapper.Map<FlightDTO>(ticket.Flight)
      };
    }
  }
}