using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.AddPet
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