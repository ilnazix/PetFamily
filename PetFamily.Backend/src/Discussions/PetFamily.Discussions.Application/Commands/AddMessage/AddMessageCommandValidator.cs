using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.AddMessage;

public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand> 
{
    public AddMessageCommandValidator()
    {
        RuleFor(c => c.DiscussionId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.UserId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Text).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
