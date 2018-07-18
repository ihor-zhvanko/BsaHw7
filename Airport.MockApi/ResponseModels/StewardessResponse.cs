using System;

namespace Airport.MockApi.ResponseModels
{
  public class StewardessResponse
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int? CrewId { get; set; }
  }
}