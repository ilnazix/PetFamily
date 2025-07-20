using FluentValidation;

namespace PetFamily.Application.Volunteers.Commands.DeletePermanently
{
    public class DeleteVolunteerPermanentlyCommandValidator : AbstractValidator<DeleteVolunteerPermanentlyCommand>
    {
        public DeleteVolunteerPermanentlyCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
