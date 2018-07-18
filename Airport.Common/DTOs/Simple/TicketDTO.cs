using System;

namespace Airport.Common.DTOs
{
  public class TicketDTO
  {
    public int Id { get; set; }
    public double Price { get; set; }
    public int FlightId { get; set; }
  }
}