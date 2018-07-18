using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class TicketDTOValidator : AbstractValidator<TicketDTO>
  {
    public TicketDTOValidator()
    {
      RuleFor(x => x.FlightId).NotEmpty().GreaterThan(0).WithMessage("Flight id should be greater than 0");
      RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
    }
  }
}