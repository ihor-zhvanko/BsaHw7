using System;
using System.ComponentModel.DataAnnotations;

namespace Airport.Data.Models
{
  public class PlaneType : Entity
  {
    [Required]
    public string Model { get; set; }
    [Range(1, 1000)]

    [Required]
    public int Seats { get; set; }

    [Required]
    [Range(1, 1000)] // tons
    public double Carrying { get; set; }
  }
}