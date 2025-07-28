using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites
{
    public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
    {
        public UpdateRequisitesCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();

            RuleForEach(c => c.Requisites).MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
        }
    }
}
