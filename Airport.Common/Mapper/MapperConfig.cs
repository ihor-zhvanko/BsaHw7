using System;
using AutoMapper;

using Airport.Common.DTOs;
using Airport.Data.Models;

namespace Airport.Common.Mappers
{
  public static class MapperConfig
  {
    public static void InitMappers()
    {
      Mapper.Initialize(cfg =>
      {
        cfg.CreateMap<Airhostess, AirhostessDTO>();
        cfg.CreateMap<Crew, CrewDTO>();
        cfg.CreateMap<Departure, DepartureDTO>();
        cfg.CreateMap<Flight, FlightDTO>();
        cfg.CreateMap<Pilot, PilotDTO>();
        cfg.CreateMap<Plane, PlaneDTO>();
        cfg.CreateMap<PlaneType, PlaneTypeDTO>();
        cfg.CreateMap<Ticket, TicketDTO>();

        cfg.CreateMap<AirhostessDTO, Airhostess>();
        cfg.CreateMap<CrewDTO, Crew>();
        cfg.CreateMap<DepartureDTO, Departure>();
        cfg.CreateMap<FlightDTO, Flight>();
        cfg.CreateMap<PilotDTO, Pilot>();
        cfg.CreateMap<PlaneDTO, Plane>();
        cfg.CreateMap<PlaneTypeDTO, PlaneType>();
        cfg.CreateMap<TicketDTO, Ticket>();

        //cfg.CreateMap<Plane>
      });
    }
  }
}