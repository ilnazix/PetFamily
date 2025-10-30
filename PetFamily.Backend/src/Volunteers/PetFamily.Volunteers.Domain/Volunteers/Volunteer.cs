﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Domain.Volunteers;

public class Volunteer : SoftDeleteableEntity<VolunteerId>
{
    private readonly List<Pet> _pets = new();

    //ef core
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(VolunteerId id, FullName fullName, Email email, PhoneNumber phoneNumber) : base(id)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Description? Description { get; private set; }
    public Experience WorkExperienceInYears { get; set; } = Experience.Default();
    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyList<Pet> Pets => _pets;

    public int PetsFoundHomeCount => _pets.Where(p => p.Status == PetStatus.FoundHome).Count();
    public int HomelessPetsCount => _pets.Where(p => p.Status == PetStatus.SearchingForHome).Count();
    public int PetsInTreatmentCount => _pets.Where(p => p.Status == PetStatus.NeedsHelp).Count();

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Email email,
        PhoneNumber phoneNumber,
        Experience experience)
    {
        FullName = fullName;
        Description = description;
        Email = email;
        PhoneNumber = phoneNumber;
        WorkExperienceInYears = experience;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);

        if (positionResult.IsFailure)
        {
            return positionResult.Error;
        }

        pet.SetPosition(positionResult.Value);
        _pets.Add(pet);

        return UnitResult.Success<Error>();
    }

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
        {
            return Errors.General.NotFound(petId.Value);
        }

        return pet;
    }

    public UnitResult<Error> MovePet(PetId petId, Position newPosition)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        var currentPosition = pet.Position;

        if (currentPosition == newPosition)
            return UnitResult.Success<Error>();

        var adjustedPositionResult = AdjustNewPositionIfOutOfRange(newPosition);

        if (adjustedPositionResult.IsFailure)
            return adjustedPositionResult.Error;

        newPosition = adjustedPositionResult.Value;

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);

        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.SetPosition(newPosition);

        return UnitResult.Success<Error>();
    }

    public override void Delete()
    {
        base.Delete();

        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }

    public override void Restore()
    {
        base.Restore();

        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    public void DeleteExpiredPets(TimeSpan lifetimeSpan)
    {
        _pets.RemoveAll(pet => pet.DeletedAt != null
            && DateTime.UtcNow > pet.DeletedAt.Value
                .Add(lifetimeSpan));
    }

    public UnitResult<Error> DeletePet(PetId petId)
    {
        var petResult = GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error;

        var pet = petResult.Value;
        pet.Delete();
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetPetMainPhoto(
        PetId petId,
        string imagePath)
    {
        var petResult = GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error;

        var pet = petResult.Value;
        return pet.SetMainPhoto(imagePath);
    }

    public UnitResult<Error> DeletePetPermanently(PetId petId)
    {
        var petResult = GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error;

        var pet = petResult.Value;
        _pets.Remove(pet);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdatePetInfo(
        PetId petId,
        PetName name,
        PetType petType,
        Description description,
        PhoneNumber ownerPhoneNumber,
        Color color,
        IReadOnlyList<Requisite> requisites,
        MedicalInformation medicalInformation,
        Address address,
        DateTime dateOfBirth)
    {
        var petResult = GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error;

        var pet = petResult.Value;

        return pet.UpdateInfo(name,
            petType,
            description,
            ownerPhoneNumber,
            color,
            requisites,
            medicalInformation,
            address,
            dateOfBirth);
    }

    public UnitResult<Error> UpdatePetStatus(
        PetId petId,
        PetStatus status)
    {
        var petResult = GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error;

        var pet = petResult.Value;

        return pet.UpdateStatus(status);
    }

    public UnitResult<Error> SetPetPhotos(PetId petId, IEnumerable<Photo> photos)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet == null)
        {
            return Errors.General.NotFound(petId.Value);
        }

        pet.SetPhotos(photos);

        return UnitResult.Success<Error>();
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var petsToMove = _pets.Where(p => p.Position.Value >= newPosition.Value
                && p.Position.Value < currentPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();

                if (result.IsFailure)
                    return result.Error;
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var petsToMove = _pets.Where(p => p.Position.Value > currentPosition.Value
                && p.Position.Value <= newPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();

                if (result.IsFailure)
                    return result.Error;
            }
        }

        return UnitResult.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count);

        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }
}
