using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class FlightDTOValidator : AbstractValidator<FlightDTO>
  {
    public FlightDTOValidator()
    {
      RuleFor(x => x.Number).NotEmpty().Length(6).WithMessage("Number length should be equal 6");
      RuleFor(x => x.ArrivalPlace).NotEmpty().WithMessage("Arrival place should not be empty");
      RuleFor(x => x.ArrivalTime).NotEmpty().WithMessage("Arrival time should not be empty");
      RuleFor(x => x.DeparturePlace).NotEmpty().WithMessage("Departure place should not be empty");
      RuleFor(x => x.DepartureTime).NotEmpty().WithMessage("Departure time should not be empty");
    }
  }
}