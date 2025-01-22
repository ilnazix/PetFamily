using FluentValidation;

namespace PetFamily.Application.Volunteers.HardDelete
{
    public class HardDeleteCommandValidator : AbstractValidator<HardDeleteCommand> 
    {
        public HardDeleteCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
