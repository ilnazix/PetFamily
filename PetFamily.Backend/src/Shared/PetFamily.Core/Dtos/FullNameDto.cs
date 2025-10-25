namespace PetFamily.Volunteers.Application.Volunteers.Commands.Shared;

public record FullNameDto(
        string FirstName,
        string LastName,
        string MiddleName
    );
