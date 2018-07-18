using System;

namespace Airport.Common.DTOs
{
  public class PilotDTO
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public double Experience { get; set; }

  }
}