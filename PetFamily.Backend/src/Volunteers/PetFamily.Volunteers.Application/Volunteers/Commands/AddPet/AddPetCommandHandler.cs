using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Volunteers.Domain.Volunteers;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;

public class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly IVolunteersUnitOfWork _unitOfWork;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ISpeciesModule _speciesModule;
    private readonly ILogger<AddPetCommandHandler> _logger;

    public AddPetCommandHandler(
        IVolunteersUnitOfWork unitOfWork,
        IValidator<AddPetCommand> validator,
        ISpeciesModule speciesModule,
        ILogger<AddPetCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _speciesModule = speciesModule;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();


        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var checkBreedExistenceRequest = new CheckBreedExistenceRequest(command.SpeciesId, command.BreeedId); 
        var isSpeciesAndBreedExist = await _speciesModule
                .CheckBreedsExistence(checkBreedExistenceRequest, cancellationToken);

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

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Add pet (Id={petId}) to volunteer (Id={volunteerId})", petId.Value, volunteerId.Value);

        return petId.Value;
    }
}
