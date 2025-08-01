using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Species.Contracts;
using PetFamily.Volunteers.Domain.Volunteers;
using System.Threading;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetInfo
{
    public class UpdatePetInfoCommandHandler : ICommandHandler<Guid, UpdatePetInfoCommand>
    {
        private readonly IValidator<UpdatePetInfoCommand> _validator;
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly ISpeciesModule _speciesModule;
        private readonly ILogger<UpdatePetInfoCommand> _logger;

        public UpdatePetInfoCommandHandler(
            IValidator<UpdatePetInfoCommand> validator,
            IVolunteersUnitOfWork unitOfWork,
            ISpeciesModule speciesModule,
            ILogger<UpdatePetInfoCommand> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _speciesModule = speciesModule;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdatePetInfoCommand command, 
            CancellationToken cancelationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancelationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(volunteerId, cancelationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var checkBreedExistenceRequest = new CheckBreedExistenceRequest(command.SpeciesId, command.BreedId);
            var isSpeciesAndBreedExist = await _speciesModule
                    .CheckBreedsExistence(checkBreedExistenceRequest, cancelationToken);

            if (!isSpeciesAndBreedExist)
                return Errors.Pets.InvalidSpeciesOrBreed().ToErrorList();

            var volunteer = volunteerResult.Value;

            var petId = PetId.Create(command.PetId);
            var petName = PetName.Create(command.PetName).Value;
            var speciesId = SpeciesId.Create(command.SpeciesId);
            var petType = PetType.Create(speciesId, command.BreedId).Value;
            var description = Description.Create(command.Description).Value;
            var ownerPhoneNumber = PhoneNumber.Create(command.OwnerPhoneNumber).Value;
            var color = Color.Create(command.Color).Value;
            var requisites = GetRequisites(command);
            var medicalInfo = GetMedicalInfo(command);
            var address = GetAddress(command);
            var dateOfBirth = command.DateOfBirth;

            var updateResult = volunteer.UpdatePetInfo(
                petId,
                petName,
                petType,
                description,
                ownerPhoneNumber,
                color,
                requisites,
                medicalInfo,
                address,
                dateOfBirth);

            if (updateResult.IsFailure)
                return updateResult.Error.ToErrorList();

            _logger.LogInformation(
                "Pet information with ID {PetId} was successfully updated by volunteer with ID {VolunteerId}",
                petId.Value,
                volunteerId.Value
            );

            await _unitOfWork.Commit(cancelationToken);

            return petId.Value;
        }

        private Address GetAddress(UpdatePetInfoCommand command)
        {
            var address = Address.Create(
                command.Country,
                command.State,
                command.City,
                command.Street,
                command.HouseNumber,
                command.ApartmentNumber
            ).Value;

            return address;
        }

        private MedicalInformation GetMedicalInfo(UpdatePetInfoCommand command)
        {
            var medicalInfo = MedicalInformation.Create(
                command.HealthInformation,
                command.Height,
                command.Weight,
                command.IsVaccinated,
                command.IsCastrated
            ).Value;

            return medicalInfo;
        }

        private List<Requisite> GetRequisites(UpdatePetInfoCommand command)
        {
            var requisites = command.Requisites
                .Select(r => Requisite.Create(r.Title, r.Description).Value)
                .ToList();

            return requisites;
        }
    }
}
