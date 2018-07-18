using System;
using AutoMapper;

using Airport.Data.Models;

namespace Airport.Common.DTOs
{
  public class PlaneDetailsDTO
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public TimeSpan ServiceLife { get; set; }

    public PlaneTypeDTO PlaneType { get; set; }

    public static PlaneDetailsDTO Create(Plane plane)
    {
      return new PlaneDetailsDTO
      {
        Id = plane.Id,
        Name = plane.Name,
        ReleaseDate = plane.ReleaseDate,
        ServiceLife = plane.ServiceLife,
        PlaneType = Mapper.Map<PlaneTypeDTO>(plane.PlaneType)
      };
    }
  }
}