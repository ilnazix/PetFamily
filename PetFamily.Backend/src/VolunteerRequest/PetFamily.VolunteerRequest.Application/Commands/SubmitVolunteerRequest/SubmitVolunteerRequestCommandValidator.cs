using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequest.Application.Commands.SubmitVolunteerRequest;

public class SubmitVolunteerRequestCommandValidator : AbstractValidator<SubmitVolunteerRequestCommand> 
{
    public SubmitVolunteerRequestCommandValidator()
    {
        RuleFor(c => c.VolunteerRequestId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.UserId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

