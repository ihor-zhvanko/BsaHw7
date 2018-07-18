using System;

namespace Airport.Common.DTOs
{
  public class DepartureDTO
  {
    public int Id { get; set; }
    public int FlightId { get; set; }
    public DateTime Date { get; set; }
    public int CrewId { get; set; }
    public int PlaneId { get; set; }
  }
}