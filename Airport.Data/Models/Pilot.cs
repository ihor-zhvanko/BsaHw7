using System;
using System.ComponentModel.DataAnnotations;

namespace Airport.Data.Models
{
  public class Pilot : Entity
  {
    [Required]
    [StringLength(32)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(32)]
    public string LastName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [Range(0, 100)]
    public double Experience { get; set; }

  }
}