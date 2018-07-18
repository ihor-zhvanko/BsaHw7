using System;
using FluentValidation;

using Airport.Common.InputModels;

namespace Airport.Common.Validation
{
  public class CrewInputModelValidator : AbstractValidator<CrewInputModel>
  {
    public CrewInputModelValidator()
    {
      RuleFor(x => x.PilotId).GreaterThan(0).WithMessage("Pilot Id should be greater than 0");
      RuleFor(x => x.AirhostessIds).NotEmpty().WithMessage("Unable to create crew without airhostesses");
    }
  }
}