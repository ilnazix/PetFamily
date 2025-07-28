using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedias
{
    public class UpdateSocialMediasCommandValidator : AbstractValidator<UpdateSocialMediasCommand>
    {
        public UpdateSocialMediasCommandValidator()
        {
            RuleFor(c => c.VolunteerId).NotEmpty();
            RuleForEach(c => c.SocialMedias).MustBeValueObject(smq => SocialMedia.Create(smq.Link, smq.Title));
        }
    }
}
