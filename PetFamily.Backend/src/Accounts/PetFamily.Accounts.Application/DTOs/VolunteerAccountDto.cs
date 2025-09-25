using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.DTOs;

public class VolunteerAccountDto
{
    public Guid Id { get; set; }
    public List<Requisite> Requisites { get; set; } = new List<Requisite>();
    public List<SocialMedia> SocialMedias { get; set; } = new List<SocialMedia>();
}