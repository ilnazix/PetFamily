using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPetPhoto
{
    public record AddPetPhotoCommand(
        Guid VolunteerId, 
        Guid PetId, 
        IEnumerable<UploadFileCommand> Files) : ICommand;
}
