using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Discussions.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionCommandValidator()
    {
        RuleFor(c => c.RelationId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Participants).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(c => c.Participants).MustBeValueObject(p => User.Create(p.Id, p.Email));
    }
}
