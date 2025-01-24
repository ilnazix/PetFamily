using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateSocialMedias
{
    public class UpdateSocialMediasCommandValidator : AbstractValidator<UpdateSocialMediasCommand>
    {
        public UpdateSocialMediasCommandValidator()
        {
            RuleFor(c => c.VolunteerId).NotEmpty();
            RuleForEach(c => c.Dto.SocialMedias).MustBeValueObject(smq => SocialMedia.Create(smq.Link, smq.Title));
        }
    }
}
