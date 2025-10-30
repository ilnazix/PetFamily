using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Volunteers.Domain.Volunteers;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(c => c.Status).MustBeValueObject(PetStatus.Create);
        RuleFor(c => c.VolunteerId).NotEmpty();
        RuleFor(c => c.PetId).NotEmpty();
    }
}
