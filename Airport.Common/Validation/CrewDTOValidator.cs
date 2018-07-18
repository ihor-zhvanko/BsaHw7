using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class CrewDTOValidator : AbstractValidator<CrewDTO>
  {
    public CrewDTOValidator()
    {
      RuleFor(x => x.PilotId).GreaterThan(0).WithMessage("Pilot Id should be greater than 0");
    }
  }
}