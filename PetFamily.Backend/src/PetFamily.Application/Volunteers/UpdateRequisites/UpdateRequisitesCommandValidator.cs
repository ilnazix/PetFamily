using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateRequisites
{
    public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
    {
        public UpdateRequisitesCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();

            RuleForEach(c => c.Dto.Requisites).MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
        }
    }
}
