using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class DepartureDTOValidator : AbstractValidator<DepartureDTO>
  {
    public DepartureDTOValidator()
    {
      RuleFor(x => x.CrewId).GreaterThan(0).WithMessage("Crew Id should be greater than 0");
      RuleFor(x => x.Date).NotEmpty().WithMessage("Date should not be empty");
      RuleFor(x => x.Date).LessThan(DateTime.Now).WithMessage("Date should be less than current");
      RuleFor(x => x.FlightId).GreaterThan(0).WithMessage("Flight Id should be greater than 0");
      RuleFor(x => x.PlaneId).GreaterThan(0).WithMessage("Plane Id should be greater than 0");
    }
  }
}