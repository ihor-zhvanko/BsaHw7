using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport.Data.Models
{
  public class Ticket : Entity
  {
    [Required]
    [Range(0, Double.MaxValue)]
    public double Price { get; set; }

    public int FlightId { get; set; }

    public Flight Flight { get; set; }
  }
}