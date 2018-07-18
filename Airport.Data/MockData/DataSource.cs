using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

using Airport.Data.Models;

namespace Airport.Data.MockData
{
  public class DataSource
  {
    public IList<Airhostess> Airhostess { get; }
    public IList<Crew> Crew { get; }
    public IList<Departure> Depature { get; }
    public IList<Flight> Flight { get; }
    public IList<Pilot> Pilot { get; }
    public IList<Plane> Plane { get; }
    public IList<PlaneType> PlaneType { get; }
    public IList<Ticket> Ticket { get; }

    public DataSource()
    {
      try
      {
        Airhostess = Read<Airhostess>();
        Crew = Read<Crew>();
        Depature = Read<Departure>();
        Flight = Read<Flight>();
        Pilot = Read<Pilot>();
        Plane = Read<Plane>();
        PlaneType = Read<PlaneType>();
        Ticket = Read<Ticket>();
      }
      catch (Exception e)
      {
        Airhostess = new List<Airhostess>();
        Crew = new List<Crew>();
        Depature = new List<Departure>();
        Flight = new List<Flight>();
        Pilot = new List<Pilot>();
        Plane = new List<Plane>();
        PlaneType = new List<PlaneType>();
        Ticket = new List<Ticket>();
        Console.WriteLine("DataSource: Failed to load data {0}", e.Message);
      }
    }

    private IList<TModel> Read<TModel>()
    {
      var filename = typeof(TModel).Name + ".json";
      var text = File.ReadAllText("/Seed/" + filename);
      return JsonConvert.DeserializeObject<IList<TModel>>(text);
    }


    public IList<TModel> Get<TModel>()
    {
      if (typeof(TModel) == typeof(Airhostess))
      {
        return (IList<TModel>)Airhostess;
      }
      else if (typeof(TModel) == typeof(Crew))
      {
        return (IList<TModel>)Crew;
      }
      else if (typeof(TModel) == typeof(Departure))
      {
        return (IList<TModel>)Depature;
      }
      else if (typeof(TModel) == typeof(Flight))
      {
        return (IList<TModel>)Flight;
      }
      else if (typeof(TModel) == typeof(Pilot))
      {
        return (IList<TModel>)Pilot;
      }
      else if (typeof(TModel) == typeof(Plane))
      {
        return (IList<TModel>)Plane;
      }
      else if (typeof(TModel) == typeof(PlaneType))
      {
        return (IList<TModel>)PlaneType;
      }
      else if (typeof(TModel) == typeof(Ticket))
      {
        return (IList<TModel>)Ticket;
      }

      throw new NotImplementedException("Requested type unknown");
    }
  }
}