using CityInfo.API.Models;
using FluentValidation;

namespace CityInfo.API.validators
{
    public class PointOfInterestSaveValidator : AbstractValidator<PointOfInterestSaveDto>
    {
        public PointOfInterestSaveValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You should provide a name value.")
                                .MaximumLength(50).WithMessage("Max. length of Name should be 50.");
            RuleFor(p => p.Description).MaximumLength(200).WithMessage("Max. length of Description should be 200.");
        }
    }
}
