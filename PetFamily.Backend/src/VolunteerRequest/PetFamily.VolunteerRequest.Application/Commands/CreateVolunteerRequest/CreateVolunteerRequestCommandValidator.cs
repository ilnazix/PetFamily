using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;


namespace PetFamily.VolunteerRequest.Application.Commands.CreateVolunteerRequest;

public class CreateVolunteerRequestCommandValidator : AbstractValidator<CreateVolunteerRequestCommand> 
{
    public CreateVolunteerRequestCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty().WithError(Errors.General.ValueIsRequired());

        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(c => FullName.Create(c.FirstName, c.LastName, c.MiddleName));

        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}
