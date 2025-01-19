using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer
{
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
        public RequisitesList? Requisites { get; private set; }
        public IReadOnlyList<Pet> Pets => _pets;
        public SocialMediaList? SocialMediaList { get; private set; }

        public int PetsFoundHomeCount => _pets.Where(p => p.Status == PetStatus.FoundHome).Count();
        public int HomelessPetsCount => _pets.Where(p => p.Status == PetStatus.SearchingForHome).Count();
        public int PetsInTreatmentCount => _pets.Where(p => p.Status == PetStatus.NeedsHelp).Count();

        public void UpdateSocialMedias(IEnumerable<SocialMedia> newSocialMedias)
        {
            SocialMediaList = new SocialMediaList(newSocialMedias.ToList());
        }
        public void UpdateRequisites(IEnumerable<Requisite> newRequiesites)
        {
            Requisites = new RequisitesList(newRequiesites.ToList());
        }

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

        public override void Delete()
        {
            base.Delete();

            foreach(var pet in _pets)
            {
                pet.Delete();
            }
        }

        public override void Restore()
        {
            base.Restore();

            foreach(var pet in _pets)
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
    }
}
