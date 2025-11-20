using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string PetName,
    string Description,
    string PhoneNumber,
    string PetStatus,
    Guid SpeciesId,
    Guid BreeedId
    ) : ICommand;