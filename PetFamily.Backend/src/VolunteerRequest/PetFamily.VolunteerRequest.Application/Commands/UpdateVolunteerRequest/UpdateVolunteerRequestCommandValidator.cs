using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequest.Application.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestCommandValidator : AbstractValidator<UpdateVolunteerRequestCommand> 
{
    public UpdateVolunteerRequestCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.VolunteerRequestId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(c => FullName.Create(c.FirstName, c.LastName, c.MiddleName));
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}
