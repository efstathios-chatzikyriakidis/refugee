using System;
using FluentValidation;
using Refugee.Rest.Dto.Input;

namespace Refugee.Server.Validators
{
    public class UpdateRefugeeInputDtoValidator : AbstractValidator<UpdateRefugeeInputDto>
    {
        public UpdateRefugeeInputDtoValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithName("name").When(o => o.Name != null);

            RuleFor(o => o.Nationality).NotEmpty().WithName("nationality").When(o => o.Nationality != null);

            RuleFor(o => o.Passport).NotEmpty().WithName("passport").When(o => o.Passport != null);

            RuleFor(o => o.BirthYear).GreaterThanOrEqualTo(1900).WithName("birth year").When(o => o.BirthYear != null);

            RuleFor(o => o.BirthYear).LessThanOrEqualTo(DateTime.UtcNow.Year).WithName("birth year").When(o => o.BirthYear != null);

            RuleFor(o => o.HotSpotId).NotEmpty().WithName("hotspot identifier").When(o => o.HotSpotId != null);
        }
    }
}