using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.AddPet
{
    public class AddPetCommandHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetCommandHandler> _logger;

        public AddPetCommandHandler(
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if(validationResult.IsValid == false)
                validationResult.ToErrorList();


            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var speciesId = SpeciesId.Create(command.SpeciesId);
            var speciesResult = await _speciesRepository.GetById(speciesId, cancellationToken);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var breedId = BreedId.Create(command.BreeedId);
            var breedResult = speciesResult.Value.GetBreedById(breedId);

            if (breedResult.IsFailure)
                return breedResult.Error.ToErrorList();

            var petId = PetId.NewPetId();
            var petName = PetName.Create(command.PetName).Value;
            var description = Description.Create(command.Description).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
            var petStatus = PetStatus.Create(command.PetStatus).Value;
            var petType = PetType.Create(speciesId, breedId.Value).Value;

            var pet = new Pet(petId, petName, petType, description, phoneNumber, petStatus);

            var result = volunteerResult.Value.AddPet(pet);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Add pet (id={petId}) to volunteer (id={volunteerId})", petId.Value, volunteerId.Value);

            return petId.Value;
        }
    }
}
