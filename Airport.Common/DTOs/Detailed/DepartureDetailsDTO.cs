using System;
using Airport.Data.Models;

using AutoMapper;

namespace Airport.Common.DTOs
{
  public class DepartureDetailsDTO
  {
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public FlightDTO Flight { get; set; }
    public PlaneDetailsDTO Plane { get; set; }
    public CrewDetailsDTO Crew { get; set; }

    public static DepartureDetailsDTO Create(
      Departure departure, PlaneDetailsDTO plane, CrewDetailsDTO crew
    )
    {
      return new DepartureDetailsDTO
      {
        Id = departure.Id,
        Date = departure.Date,
        Flight = Mapper.Map<FlightDTO>(departure.Flight),
        Plane = plane,
        Crew = crew
      };
    }

  }
}