using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.ChangePetPosition
{
    public class ChangePetPositionCommandValidator : AbstractValidator<ChangePetPositionCommand>
    {
        public ChangePetPositionCommandValidator()
        {
            RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleFor(c => c.NewPosition).MustBeValueObject(Position.Create);
        }
    }
}