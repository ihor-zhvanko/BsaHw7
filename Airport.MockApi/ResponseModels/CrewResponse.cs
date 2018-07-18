using System;
using System.Collections.Generic;

namespace Airport.MockApi.ResponseModels
{
  public class CrewResponse
  {
    public int Id { get; set; }
    public IList<StewardessResponse> Stewardess { get; set; }
    public IList<PilotResponse> Pilot { get; set; }
  }
}