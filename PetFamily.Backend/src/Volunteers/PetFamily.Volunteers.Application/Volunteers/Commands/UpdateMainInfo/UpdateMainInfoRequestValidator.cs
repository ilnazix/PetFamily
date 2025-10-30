using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

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
