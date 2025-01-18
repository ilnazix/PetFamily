using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoRequestValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Dto.Email).MustBeValueObject(Email.Create);
            RuleFor(r => r.Dto.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
            RuleFor(r => r.Dto.Description).MustBeValueObject(Description.Create);
            RuleFor(r => r.Dto.Experience).MustBeValueObject(Experience.Create);

            RuleFor(r => r.Dto.FullName)
                .MustBeValueObject(fn => FullName.Create(fn.FirstName, fn.LastName, fn.MiddleName));
        }
    }
}
