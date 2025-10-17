using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionCommandValidator()
    {
        RuleFor(c => c.RelationId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.ParticipantIds).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
