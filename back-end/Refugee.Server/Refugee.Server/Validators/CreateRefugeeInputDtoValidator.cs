using System;
using FluentValidation;
using Refugee.Rest.Dto.Input;

namespace Refugee.Server.Validators
{
    public class CreateRefugeeInputDtoValidator : AbstractValidator<CreateRefugeeInputDto>
    {
        public CreateRefugeeInputDtoValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithName("name");

            RuleFor(o => o.Nationality).NotEmpty().WithName("nationality");

            RuleFor(o => o.Passport).NotEmpty().WithName("passport");

            RuleFor(o => o.BirthYear).GreaterThanOrEqualTo(1900).WithName("birth year");

            RuleFor(o => o.BirthYear).LessThanOrEqualTo(DateTime.UtcNow.Year).WithName("birth year");

            RuleFor(o => o.HotSpotId).NotEmpty().WithName("hotspot identifier");
        }
    }
}