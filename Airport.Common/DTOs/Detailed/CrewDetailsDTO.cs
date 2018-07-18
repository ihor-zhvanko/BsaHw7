using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Airport.Data.Models;

namespace Airport.Common.DTOs
{
  public class CrewDetailsDTO
  {
    public int Id { get; set; }
    public PilotDTO Pilot { get; set; }
    public IList<AirhostessDTO> Airhostesses { get; set; }

    public static CrewDetailsDTO Create(Crew crew)
    {
      return new CrewDetailsDTO
      {
        Id = crew.Id,
        Pilot = Mapper.Map<PilotDTO>(crew.Pilot),
        Airhostesses = Mapper.Map<IList<AirhostessDTO>>(crew.Airhostesses)
      };
    }
  }
}