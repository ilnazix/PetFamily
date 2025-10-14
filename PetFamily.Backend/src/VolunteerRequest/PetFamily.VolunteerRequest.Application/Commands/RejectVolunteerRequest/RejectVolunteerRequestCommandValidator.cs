using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.VolunteerRequest.Application.Commands.RejectVolunteerRequest;

public class RejectVolunteerRequestCommandValidator : AbstractValidator<RejectVolunteerRequestCommand>
{
    public RejectVolunteerRequestCommandValidator()
    {
        RuleFor(c => c.VolunteerRequestId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.RejectionComment).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.AdminId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
