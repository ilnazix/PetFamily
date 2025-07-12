using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdatePetStatus
{
    public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
    {
        public UpdatePetStatusCommandValidator()
        {
            RuleFor(c => c.Status).MustBeValueObject(PetStatus.Create);
            RuleFor(c => c.VolunteerId).NotEmpty();
            RuleFor(c => c.PetId).NotEmpty();
        }
    }
}
