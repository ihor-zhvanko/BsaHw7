using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport.Data.Models
{
  public class Crew : Entity
  {
    public int PilotId { get; set; }
    public IList<Airhostess> Airhostesses { get; set; }

    public Pilot Pilot { get; set; }
  }
}