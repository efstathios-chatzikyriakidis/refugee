using FluentValidation;
using Refugee.Rest.Dto.Input;

namespace Refugee.Server.Validators
{
    public class UpdateHotSpotInputDtoValidator : AbstractValidator<UpdateHotSpotInputDto>
    {
        public UpdateHotSpotInputDtoValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithName("name").When(o => o.Name != null);

            RuleFor(o => o.Latitude).GreaterThanOrEqualTo(-90).WithName("latitude").When(o => o.Latitude != null);

            RuleFor(o => o.Latitude).LessThanOrEqualTo(90).WithName("latitude").When(o => o.Latitude != null);

            RuleFor(o => o.Longitude).GreaterThanOrEqualTo(-180).WithName("longitude").When(o => o.Longitude != null);

            RuleFor(o => o.Longitude).LessThanOrEqualTo(180).WithName("longitude").When(o => o.Longitude != null);
        }
    }
}