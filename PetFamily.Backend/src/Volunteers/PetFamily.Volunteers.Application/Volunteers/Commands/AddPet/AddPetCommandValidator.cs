using FluentValidation;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.Volunteers;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        public AddPetCommandValidator()
        {
            RuleFor(c => c.PetName).MustBeValueObject(PetName.Create);
            RuleFor(c => c.Description).MustBeValueObject(Description.Create);
            RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
            RuleFor(c => c.PetStatus).MustBeValueObject(PetStatus.Create);
            RuleFor(c => c.SpeciesId).NotEmpty();
            RuleFor(c => c.BreeedId).NotEmpty();
            RuleFor(c => c.VolunteerId).NotEmpty();
        }
    }
}