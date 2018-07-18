using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class PlaneTypeDTOValidator : AbstractValidator<PlaneTypeDTO>
  {
    public PlaneTypeDTOValidator()
    {
      RuleFor(x => x.Model).NotEmpty().WithMessage("Model should not be empty");
      RuleFor(x => x.Seats).GreaterThan(0).WithMessage("Seats count should be greater than 0");
      RuleFor(x => x.Carrying).GreaterThan(0).WithMessage("Carrying should be greater than 0");
    }
  }
}