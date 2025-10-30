using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class VolunteerAccount 
{
    public const string ROLE = "volunteer";
    public Guid Id { get; set; }

    public IReadOnlyList<Requisite> Requisites { get; private set; } = new List<Requisite>();
    public IReadOnlyList<SocialMedia> SocialMedias { get; private set; } = new List<SocialMedia>();

    public void UpdateSocialMedias(IEnumerable<SocialMedia> newSocialMedias)
    {
        SocialMedias = newSocialMedias.ToList();
    }
    public void UpdateRequisites(IEnumerable<Requisite> newRequiesites)
    {
        Requisites = newRequiesites.ToList();
    }
}