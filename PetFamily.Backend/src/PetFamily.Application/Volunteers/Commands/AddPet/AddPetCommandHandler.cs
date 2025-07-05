using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species.Commands;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.AddPet
{
    public class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetCommandHandler> _logger;

        public AddPetCommandHandler(
            IVolunteersRepository volunteersRepository,
            IReadDbContext readDbContext,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _readDbContext = readDbContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                validationResult.ToErrorList();


            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var isSpeciesAndBreedExist = await _readDbContext.Breeds
                .AnyAsync(b => b.Id == command.BreeedId && b.SpeciesId == command.SpeciesId, cancellationToken);

            if (!isSpeciesAndBreedExist)
                return Errors.Pets.InvalidSpeciesOrBreed().ToErrorList();

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
