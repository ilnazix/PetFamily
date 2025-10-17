using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public class CloseDiscussionCommandValidator : AbstractValidator<CloseDiscussionCommand>
{
    public CloseDiscussionCommandValidator()
    {
        RuleFor(c => c.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}

