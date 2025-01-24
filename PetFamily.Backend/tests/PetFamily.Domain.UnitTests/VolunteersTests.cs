using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Domain.Tests
{
    public class VolunteersTests
    {
        [Fact]
        public void AddPet_WhenFirstPetIsAdded_ShouldAssignPositionOne()
        {
            //arrange
            var petId = PetId.NewPetId();
            var petName = PetName.Create("Lucy").Value;
            var petType = PetType.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value;
            var description = Description.Create("A friendly Labrador").Value;
            var ownerPhoneNumber = PhoneNumber.Create("+1234567890").Value;
            var petStatus = PetStatus.NeedsHelp;
            var pet = new Pet(petId, petName, petType, description, ownerPhoneNumber, petStatus);

            var volunteerId = VolunteerId.NewVolunteerId();
            var fullName = FullName.Create("John", "Doe", "Smith").Value;
            var email = Email.Create("johndoe@example.com").Value;
            var phoneNumber = PhoneNumber.Create("+9876543210").Value;
            var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);

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
            var petName = PetName.Create("Lucy").Value;
            var petType = PetType.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value;
            var description = Description.Create("A friendly Labrador").Value;
            var ownerPhoneNumber = PhoneNumber.Create("+1234567890").Value;
            var petStatus = PetStatus.NeedsHelp;
            var pets = Enumerable.Range(0, petsCount).Select(x => new Pet(PetId.NewPetId(), petName, petType, description, ownerPhoneNumber, petStatus));

            var petToAddId = PetId.NewPetId();
            var petToAdd = new Pet(petToAddId, petName, petType, description, ownerPhoneNumber, petStatus);

            var volunteerId = VolunteerId.NewVolunteerId();
            var fullName = FullName.Create("John", "Doe", "Smith").Value;
            var email = Email.Create("johndoe@example.com").Value;
            var phoneNumber = PhoneNumber.Create("+9876543210").Value;
            var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);


            foreach (var pet in pets)
            {
                volunteer.AddPet(pet);
            }

            //act
            var result = volunteer.AddPet(petToAdd);

            //assert
            var addedPetResult = volunteer.GetPetById(petToAddId);
            var expectedPosition = Position.Create(petsCount + 1).Value;
            Assert.True(result.IsSuccess);
            Assert.True(addedPetResult.IsSuccess);
            Assert.Equal(expectedPosition, addedPetResult.Value.Position);
        }
    }
}
