using FluentValidation;

namespace PetFamily.Application.Volunteers.Commands.HardDelete
{
    public class HardDeleteCommandValidator : AbstractValidator<HardDeleteCommand>
    {
        public HardDeleteCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
