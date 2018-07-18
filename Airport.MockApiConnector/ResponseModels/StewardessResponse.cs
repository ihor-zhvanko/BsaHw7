using System;

namespace Airport.MockApiConnector.ResponseModels
{
  public class StewardessResponse
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int? CrewId { get; set; }
  }
}