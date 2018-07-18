using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class AirhostessDTOValidator : AbstractValidator<AirhostessDTO>
  {
    public AirhostessDTOValidator()
    {
      RuleFor(x => x.CrewId).GreaterThan(0).WithMessage("Crew Id should be greater than 0");
      RuleFor(x => x.BirthDate).NotEmpty();
      RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name should not be empty");
      RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name should not be empty");
    }
  }
}