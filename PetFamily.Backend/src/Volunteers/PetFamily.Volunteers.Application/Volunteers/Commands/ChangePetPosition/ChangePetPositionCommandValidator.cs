using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.ChangePetPosition
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