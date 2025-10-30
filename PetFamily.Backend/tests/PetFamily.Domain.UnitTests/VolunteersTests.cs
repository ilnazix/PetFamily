using FluentAssertions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Volunteers;

namespace PetFamily.Domain.UnitTests;

public class VolunteersTests
{
    [Fact]
    public void AddPet_WhenFirstPetIsAdded_ShouldAssignPositionOne()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(0);

        var petId = PetId.NewPetId();
        var petName = PetName.Create("Lucy").Value;
        var petType = PetType.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value;
        var description = Description.Create("A friendly Labrador").Value;
        var ownerPhoneNumber = PhoneNumber.Create("+1234567890").Value;
        var petStatus = PetStatus.NeedsHelp;
        var pet = new Pet(petId, petName, petType, description, ownerPhoneNumber, petStatus);

        //act
        var result = volunteer.AddPet(pet);

        //assert
        var addedPetResult = volunteer.GetPetById(petId);

        Assert.True(result.IsSuccess);
        Assert.True(addedPetResult.IsSuccess);
        Assert.Equal(petId, addedPetResult.Value.Id);
        Assert.Equal(addedPetResult.Value.Position, Position.Create(1).Value);
    }

    [Fact]
    public void AddPet_WhenAddingNewPet_ShouldAssignCorrectLastPosition()
    {
        //arrange
        var petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);

        var petName = PetName.Create("Lucy").Value;
        var petType = PetType.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value;
        var description = Description.Create("A friendly Labrador").Value;
        var ownerPhoneNumber = PhoneNumber.Create("+1234567890").Value;
        var petStatus = PetStatus.NeedsHelp;

        var petToAddId = PetId.NewPetId();
        var petToAdd = new Pet(petToAddId, petName, petType, description, ownerPhoneNumber, petStatus);

        //act
        var result = volunteer.AddPet(petToAdd);

        //assert
        var addedPetResult = volunteer.GetPetById(petToAddId);
        var expectedPosition = Position.Create(petsCount + 1).Value;
        Assert.True(result.IsSuccess);
        Assert.True(addedPetResult.IsSuccess);
        Assert.Equal(expectedPosition, addedPetResult.Value.Position);
    }

    [Fact]
    public void MovePet_WhenMovingAtTheSamePosition_ShouldNotMove()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(2).Value;
        var positionTo = Position.Create(2).Value;

        var petToMove = volunteer.Pets.First(p => p.Position == positionFrom);
        var petsBeforeMove = volunteer.Pets.OrderBy(p => p.Position).ToList();

        //Act
        var result = volunteer.MovePet(petToMove.Id, positionTo);

        //Assert
        result.IsSuccess.Should().BeTrue();

        var petsAfterMove = volunteer.Pets.OrderBy(p => p.Position).ToList();
        petsAfterMove.Should().BeEquivalentTo(petsBeforeMove, options => options.WithStrictOrdering());

        var movedPet = petsAfterMove.First(p => p.Id == petToMove.Id);
        movedPet.Position.Should().Be(positionFrom);
    }

    [Fact]
    public void MovePet_WhenMovingForward_ShouldMoveOtherPetsBack()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(2).Value;
        var positionTo = Position.Create(4).Value;

        var petToMove = volunteer.Pets.First(p => p.Position == positionFrom);

        var petsBeforeMove = volunteer.Pets.OrderBy(p => p.Position).ToList();
        var firstPet = petsBeforeMove[0];
        var secondPet = petsBeforeMove[1];
        var thirdPet = petsBeforeMove[2];
        var thourthPet = petsBeforeMove[3];
        var fifthPet = petsBeforeMove[4];

        //Act
        var result = volunteer.MovePet(petToMove.Id, positionTo);

        //Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(positionTo);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        thourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }

    [Fact]
    public void MovePet_WhenMovingFirstPetToLastPosition_ShouldMoveOtherPetsBack()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(1).Value;
        var positionTo = Position.Create(5).Value;

        var petToMove = volunteer.Pets.First(p => p.Position == positionFrom);

        var petsBeforeMove = volunteer.Pets.OrderBy(p => p.Position).ToList();
        var firstPet = petsBeforeMove[0];
        var secondPet = petsBeforeMove[1];
        var thirdPet = petsBeforeMove[2];
        var thourthPet = petsBeforeMove[3];
        var fifthPet = petsBeforeMove[4];

        //Act
        var result = volunteer.MovePet(petToMove.Id, positionTo);

        //Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(positionTo);
        secondPet.Position.Should().Be(Position.Create(1).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        thourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
    }

    [Fact]
    public void MovePet_WhenMovingLastPetToFirstPosition_ShouldMoveOtherPetsBack()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(5).Value;
        var positionTo = Position.Create(1).Value;

        var petToMove = volunteer.Pets.First(p => p.Position == positionFrom);

        var petsBeforeMove = volunteer.Pets.OrderBy(p => p.Position).ToList();
        var firstPet = petsBeforeMove[0];
        var secondPet = petsBeforeMove[1];
        var thirdPet = petsBeforeMove[2];
        var thourthPet = petsBeforeMove[3];
        var fifthPet = petsBeforeMove[4];

        //Act
        var result = volunteer.MovePet(petToMove.Id, positionTo);

        //Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(2).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        thourthPet.Position.Should().Be(Position.Create(5).Value);
        fifthPet.Position.Should().Be(positionTo);
    }

    [Fact]
    public void MovePet_WhenTryToNotExistingPet_ShouldReturnReturnNotFoundError()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(5).Value;
        var positionTo = Position.Create(1).Value;
        var notExistingPetId = PetId.NewPetId();

        //Act
        var result = volunteer.MovePet(notExistingPetId, positionTo);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.NotFound(notExistingPetId.Value));
    }

    [Fact]
    public void MovePet_WhenMovingPetAtTooLargePosition_ShouldMovePetAtLastPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(3).Value;
        var positionTo = Position.Create(8).Value;

        var petToMove = volunteer.Pets.First(p => p.Position == positionFrom);

        var petsBeforeMove = volunteer.Pets.OrderBy(p => p.Position).ToList();
        var firstPet = petsBeforeMove[0];
        var secondPet = petsBeforeMove[1];
        var thirdPet = petsBeforeMove[2];
        var thourthPet = petsBeforeMove[3];
        var fifthPet = petsBeforeMove[4];

        //Act
        var result = volunteer.MovePet(petToMove.Id, positionTo);

        //Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(2).Value);
        thirdPet.Position.Should().Be(Position.Create(5).Value);
        thourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
    }

    [Fact]
    public void MovePet_WhenMovingBack_ShouldMoveOtherPetsForward()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(5);
        var positionFrom = Position.Create(4).Value;
        var positionTo = Position.Create(2).Value;

        var petToMove = volunteer.Pets.First(p => p.Position == positionFrom);
        var petsBeforeMove = volunteer.Pets.OrderBy(p => p.Position).ToList();
        var firstPet = petsBeforeMove[0];
        var secondPet = petsBeforeMove[1];
        var thirdPet = petsBeforeMove[2];
        var thourthPet = petsBeforeMove[3];
        var fifthPet = petsBeforeMove[4];

        //Act
        var result = volunteer.MovePet(petToMove.Id, positionTo);

        //Assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        thourthPet.Position.Should().Be(positionTo);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }

    private Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var petName = PetName.Create("Lucy").Value;
        var petType = PetType.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value;
        var description = Description.Create("A friendly Labrador").Value;
        var ownerPhoneNumber = PhoneNumber.Create("+1234567890").Value;
        var petStatus = PetStatus.NeedsHelp;
        var pets = Enumerable.Range(0, petsCount).Select(x => new Pet(PetId.NewPetId(), petName, petType, description, ownerPhoneNumber, petStatus));

        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("John", "Doe", "Smith").Value;
        var email = Email.Create("johndoe@example.com").Value;
        var phoneNumber = PhoneNumber.Create("+9876543210").Value;
        var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);

        foreach (var pet in pets)
        {
            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}
