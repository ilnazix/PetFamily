using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public class CloseDiscussionByRelationIdCommandValidator : AbstractValidator<CloseDiscussionByRelationIdCommand>
{
    public CloseDiscussionByRelationIdCommandValidator()
    {
        RuleFor(c => c.RelationId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}

