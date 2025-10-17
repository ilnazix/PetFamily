using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Discussions.Application.Commands.EditMessage;

public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(c => c.DiscussionId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.MessageId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.UserId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.NewText).NotEmpty().WithError(Errors.Message.MessageTextRequired());
    }
}

