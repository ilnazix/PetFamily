using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(c => c.Email).MustBeValueObject(Email.Create);

            RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

            RuleFor(c => c.FullName)
                .MustBeValueObject(c => FullName.Create(c.FirstName, c.LastName, c.MiddleName));

            RuleForEach(c => c.SocialMedias)
                .MustBeValueObject(smq => SocialMedia.Create(smq.Link, smq.Title));

            RuleForEach(c => c.Requisites)
                .MustBeValueObject(rq => Requisite.Create(rq.Title, rq.Description));
        }
    }
}
