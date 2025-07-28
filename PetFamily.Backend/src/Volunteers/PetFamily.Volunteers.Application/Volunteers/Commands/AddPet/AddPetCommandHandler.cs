using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Volunteers;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet
{
    public class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetCommandHandler> _logger;

        public AddPetCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();


            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            //TODO: ппроверить вид и породу
            /*var isSpeciesAndBreedExist = await _readDbContext.Breeds
                .AnyAsync(b => b.Id == command.BreeedId && b.SpeciesId == command.SpeciesId, cancellationToken);

            if (!isSpeciesAndBreedExist)
                return Errors.Pets.InvalidSpeciesOrBreed().ToErrorList();*/

            var petId = PetId.NewPetId();
            var petName = PetName.Create(command.PetName).Value;
            var description = Description.Create(command.Description).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
            var petStatus = PetStatus.Create(command.PetStatus).Value;
            var speciesId = SpeciesId.Create(command.SpeciesId);
            var breedId = command.BreeedId;
            var petType = PetType.Create(speciesId, breedId).Value;

            var pet = new Pet(petId, petName, petType, description, phoneNumber, petStatus);

            var result = volunteerResult.Value.AddPet(pet);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Add pet (Id={petId}) to volunteer (Id={volunteerId})", petId.Value, volunteerId.Value);

            return petId.Value;
        }
    }
}
