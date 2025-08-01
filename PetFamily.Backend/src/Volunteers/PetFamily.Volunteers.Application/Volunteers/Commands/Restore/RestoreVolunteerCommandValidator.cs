using FluentValidation;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Restore
{
    public class RestoreVolunteerCommandValidator : AbstractValidator<RestoreVolunteerCommand>
    {
        public RestoreVolunteerCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
