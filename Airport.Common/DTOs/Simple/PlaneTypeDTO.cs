using System;

namespace Airport.Common.DTOs
{
  public class PlaneTypeDTO
  {
    public int Id { get; set; }
    public string Model { get; set; }
    public int Seats { get; set; }
    public double Carrying { get; set; }
  }
}