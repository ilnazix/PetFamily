using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoRequestValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Email).MustBeValueObject(Email.Create);
            RuleFor(r => r.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
            RuleFor(r => r.Description).MustBeValueObject(Description.Create);
            RuleFor(r => r.Experience).MustBeValueObject(Experience.Create);

            RuleFor(r => r.FullName)
                .MustBeValueObject(fn => FullName.Create(fn.FirstName, fn.LastName, fn.MiddleName));
        }
    }
}
