using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdatePetInfo
{
    public class UpdatePetInfoCommandValidator : AbstractValidator<UpdatePetInfoCommand>
    {
        public UpdatePetInfoCommandValidator()
        {
            RuleFor(c => c.PetName).MustBeValueObject(PetName.Create);

            RuleFor(c => c.Description).MustBeValueObject(Description.Create);

            RuleFor(c => c.OwnerPhoneNumber).MustBeValueObject(PhoneNumber.Create);

            RuleFor(c => c.Color).MustBeValueObject(Color.Create);

            RuleForEach(c => c.Requisites)
                .MustBeValueObject(r => Requisite.Create(r.Title, r.Description));

            RuleFor(c => new
            {
                c.IsCastrated,
                c.IsVaccinated,
                c.HealthInformation,
                c.Height,
                c.Weight,
            }).MustBeValueObject(mi =>
                MedicalInformation.Create(
                    mi.HealthInformation,
                    mi.Height,
                    mi.Weight,
                    mi.IsVaccinated,
                    mi.IsCastrated));

            RuleFor(c => new
            {
                c.Country,
                c.State,
                c.City,
                c.Street,
                c.HouseNumber,
                c.ApartmentNumber
            }).MustBeValueObject(a =>
                Address.Create(
                    a.Country,
                    a.State,
                    a.City,
                    a.Street,
                    a.HouseNumber,
                    a.ApartmentNumber));

        }
    }
}
