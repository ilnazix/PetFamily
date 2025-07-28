using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.DTOs
{
    public class VolunteerDto
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string MiddleName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public RequisiteDto[] Requisites { get; init; } = [];
        public SocialMediaDto[] SocialMedias { get; init; } = [];
    }
}
