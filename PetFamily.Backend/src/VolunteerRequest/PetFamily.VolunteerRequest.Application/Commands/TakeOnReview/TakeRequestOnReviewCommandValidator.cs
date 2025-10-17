using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeOnReview;

public class TakeRequestOnReviewCommandValidator 
    : AbstractValidator<TakeRequestOnReviewCommand> 
{
    public TakeRequestOnReviewCommandValidator()
    {
        RuleFor(x => x.AdminId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(x => x.RequestId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
