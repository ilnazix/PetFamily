using FluentValidation;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePermanently;

public class DeleteVolunteerPermanentlyCommandValidator : AbstractValidator<DeleteVolunteerPermanentlyCommand>
{
    public DeleteVolunteerPermanentlyCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
