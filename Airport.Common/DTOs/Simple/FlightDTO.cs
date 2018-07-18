using System;

namespace Airport.Common.DTOs
{
  public class FlightDTO
  {
    public int Id { get; set; }
    public string Number { get; set; }
    public string DeparturePlace { get; set; }
    public DateTime DepartureTime { get; set; }
    public string ArrivalPlace { get; set; }
    public DateTime ArrivalTime { get; set; }

  }
}