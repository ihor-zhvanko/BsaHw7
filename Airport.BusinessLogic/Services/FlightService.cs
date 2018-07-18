using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;

using Airport.Common.DTOs;

using Airport.Data.Models;
using Airport.Data.UnitOfWork;

namespace Airport.BusinessLogic.Services
{
  public interface IFlightService : IService<FlightDTO>
  { }

  public class FlightService : BaseService<FlightDTO, Flight>, IFlightService
  {
    public FlightService(IUnitOfWork unitOfWork, IValidator<FlightDTO> flightDTOValidator)
      : base(unitOfWork, flightDTOValidator)
    { }
  }
}