using System;
using System.Collections.Generic;

using Airport.Common.DTOs;

namespace Airport.Common.InputModels
{
  public class CrewInputModel : CrewDTO
  {
    public IList<int> AirhostessIds { get; set; }
  }
}