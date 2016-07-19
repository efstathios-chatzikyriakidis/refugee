using FluentValidation;
using Refugee.Rest.Dto.Input;

namespace Refugee.Server.Validators
{
    public class CreateRefugeesFamilyRelationshipInputDtoValidator : AbstractValidator<CreateRefugeesFamilyRelationshipInputDto>
    {
        public CreateRefugeesFamilyRelationshipInputDtoValidator()
        {
            RuleFor(o => o.SourceId).NotEmpty().WithName("source refugee identifier");

            RuleFor(o => o.TargetId).NotEmpty().WithName("target refugee identifier");

            RuleFor(o => o.SourceId).NotEqual(o => o.TargetId).WithMessage("The source refugee identifier cannot be equal to target refugee identifier.");
        }
    }
}