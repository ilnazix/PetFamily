using FluentValidation;

namespace PetFamily.Application.Volunteers.SoftDelete
{
    public class SoftDeleteCommandValidator : AbstractValidator<SoftDeleteCommand>
    {
        public SoftDeleteCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
