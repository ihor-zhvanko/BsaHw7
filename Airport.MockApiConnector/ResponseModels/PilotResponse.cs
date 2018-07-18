using System;

namespace Airport.MockApiConnector.ResponseModels
{
  public class PilotResponse
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public double Exp { get; set; }
  }
}