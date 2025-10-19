using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.DeleteMessage;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator()
    {
        RuleFor(c => c.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(c => c.MessageId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}
