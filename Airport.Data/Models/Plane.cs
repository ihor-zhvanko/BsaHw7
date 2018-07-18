using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport.Data.Models
{
  public class Plane : Entity
  {
    [Required]
    public string Name { get; set; }
    public int PlaneTypeId { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public long ServiceLifeDays { get; set; }

    [NotMapped]
    public TimeSpan ServiceLife
    {
      get { return TimeSpan.FromDays(ServiceLifeDays); }
      set { ServiceLifeDays = value.Days; }
    }

    public PlaneType PlaneType { get; set; }

  }
}