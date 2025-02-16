using PetFamily.Application.Volunteers.Commands.AddPet;

namespace PetFamily.API.Controllers.Volunteers.Requests
{
    public record AddPetRequest
    {
        public string PetName { get; init; }
        public string Description { get; init; }
        public string PhoneNumber { get; init; }
        public string PetStatus { get; init; }
        public Guid SpeciesId { get; init; }
        public Guid BreeedId { get; init; }

        public AddPetRequest(
            string petName,
            string description,
            string phoneNumber,
            string petStatus,
            Guid speciesId,
            Guid breeedId)
        {
            PetName = petName;
            Description = description;
            PhoneNumber = phoneNumber;
            PetStatus = petStatus;
            SpeciesId = speciesId;
            BreeedId = breeedId;
        }

        public AddPetCommand ToCommand(Guid volunteerId)
        {
            return new AddPetCommand(
                volunteerId,
                PetName,
                Description,
                PhoneNumber,
                PetStatus,
                SpeciesId,
                BreeedId
            );
        }
    }

}
