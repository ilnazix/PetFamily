using FluentValidation;

namespace PetFamily.Application.Volunteers.Restore
{
    public class RestoreVolunteerCommandValidator : AbstractValidator<RestoreVolunteerCommand>
    {
        public RestoreVolunteerCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
