using FluentValidation;

namespace PetFamily.Application.Volunteers.Commands.AddPetPhoto
{
    public class AddPetPhotoCommandValidator : AbstractValidator<AddPetPhotoCommand>
    {
        public AddPetPhotoCommandValidator()
        {
            RuleFor(c => c.VolunteerId).NotEmpty();//.WithError();
            RuleFor(c => c.PetId).NotEmpty();
            RuleForEach(c => c.Files).SetValidator(new UploadPhotoCommandValidator());
        }
    }
}
