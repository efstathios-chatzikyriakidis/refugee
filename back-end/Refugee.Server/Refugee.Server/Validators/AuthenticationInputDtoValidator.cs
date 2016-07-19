using FluentValidation;
using Refugee.Rest.Dto.Input;

namespace Refugee.Server.Validators
{
    public class AuthenticationInputDtoValidator : AbstractValidator<AuthenticationInputDto>
    {
        public AuthenticationInputDtoValidator()
        {
            RuleFor(o => o.UserName).NotEmpty().WithName("username");

            RuleFor(o => o.Password).NotEmpty().WithName("password");
        }
    }
}