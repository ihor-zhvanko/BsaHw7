using System;
using FluentValidation;

using Airport.Common.DTOs;

namespace Airport.Common.Validation
{
  public class PlaneDTOValidator : AbstractValidator<PlaneDTO>
  {
    public PlaneDTOValidator()
    {
      RuleFor(x => x.Name).NotEmpty().WithMessage("Name should not be empty");
      RuleFor(x => x.PlaneTypeId).GreaterThan(0).WithMessage("Plane type id should be greater than 0");
      RuleFor(x => x.ReleaseDate).NotEmpty().WithMessage("Release date should not be empty");
      RuleFor(x => x.ReleaseDate).LessThan(DateTime.Now).WithMessage("Release date should be less than current");
      RuleFor(x => x.ServiceLife).NotEmpty().WithMessage("Service life should not be empty");
      RuleFor(x => x.ServiceLife).Must(x => x.Days > 0).WithMessage("Service life days should be greater than 0");
    }
  }
}