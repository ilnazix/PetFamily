using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.AddPetPhoto
{
    public class UploadPhotoCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadPhotoCommandValidator()
        {
            RuleFor(c => c.Content).Must(s => s.Length <= 5 * 1024 * 1024).WithError(Errors.General.ValueIsInvalid("Content")); //5МБ
            RuleFor(c => c.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired("Path"));
        }
    }
}
