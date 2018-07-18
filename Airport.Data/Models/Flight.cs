using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport.Data.Models
{
  public class Flight : Entity
  {
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Number { get; set; }

    [Required]
    [StringLength(32)]
    public string DeparturePlace { get; set; }

    [Required]
    public DateTime DepartureTime { get; set; }

    [Required]
    [StringLength(32)]
    public string ArrivalPlace { get; set; }

    [Required]
    public DateTime ArrivalTime { get; set; }
  }
}