using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Models
{
  public class Airhostess : Entity
  {
    [Required]
    [StringLength(32)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(32)]
    public string LastName { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    public int? CrewId { get; set; }

    public Crew Crew { get; set; }
  }
}