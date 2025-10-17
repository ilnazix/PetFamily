using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequest.Application.Commands.RequireRevision;

public class RequireRevisionCommandValidator : AbstractValidator<RequireRevisionCommand> 
{
    public RequireRevisionCommandValidator()
    {
        RuleFor(c => c.VolunteerRequestId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.RejectionComment).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.AdminId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
