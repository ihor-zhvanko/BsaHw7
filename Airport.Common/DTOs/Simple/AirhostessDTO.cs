using System;

namespace Airport.Common.DTOs
{
  public class AirhostessDTO
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int CrewId { get; set; }
  }
}