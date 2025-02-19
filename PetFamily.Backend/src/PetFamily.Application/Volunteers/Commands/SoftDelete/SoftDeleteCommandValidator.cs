using FluentValidation;

namespace PetFamily.Application.Volunteers.Commands.SoftDelete
{
    public class SoftDeleteCommandValidator : AbstractValidator<SoftDeleteCommand>
    {
        public SoftDeleteCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
