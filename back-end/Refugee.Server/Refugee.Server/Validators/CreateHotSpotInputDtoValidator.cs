using FluentValidation;
using Refugee.Rest.Dto.Input;

namespace Refugee.Server.Validators
{
    public class CreateHotSpotInputDtoValidator : AbstractValidator<CreateHotSpotInputDto>
    {
        public CreateHotSpotInputDtoValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithName("name");

            RuleFor(o => o.Latitude).GreaterThanOrEqualTo(-90).WithName("latitude");

            RuleFor(o => o.Latitude).LessThanOrEqualTo(90).WithName("latitude");

            RuleFor(o => o.Longitude).GreaterThanOrEqualTo(-180).WithName("longitude");

            RuleFor(o => o.Longitude).LessThanOrEqualTo(180).WithName("longitude");
        }
    }
}