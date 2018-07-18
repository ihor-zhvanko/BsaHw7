using System;
using System.Collections.Generic;

namespace Airport.MockApiConnector.ResponseModels
{
  public class CrewResponse
  {
    public IList<AirhostessResponse> Airhostesses { get; set; }
    public IList<PilotResponse> Pilot { get; set; }
  }
}