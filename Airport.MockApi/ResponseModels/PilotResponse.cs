using System;

namespace Airport.MockApi.ResponseModels
{
  public class PilotResponse
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public double Exp { get; set; }
  }
}