using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfBreedExists;
using PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfSpeciesExists;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Volunteers.Presentation.Pets.Extensions;
using PetFamily.Volunteers.Presentation.Volunteers.Extensions;

namespace PetFamily.Volunteers.Presentation;

internal class VolunteersModule : IVolunteersModule
{
    private readonly AnyPetOfBreedExistsQueryHandler _anyPetOfBreedExistsQueryHandler;
    private readonly AnyPetOfSpeciesExistsQueryHandler _anyPetOfSpeciesExistsQueryHandler;
    private readonly CreateVolunteerCommandHandler _createVolunteerCommandHandler;

    public VolunteersModule(
        AnyPetOfBreedExistsQueryHandler anyPetOfBreedExistsQueryHandler,
        AnyPetOfSpeciesExistsQueryHandler anyPetOfSpeciesExistsQueryHandler,
        CreateVolunteerCommandHandler createVolunteerCommandHandler)
    {
        _anyPetOfBreedExistsQueryHandler = anyPetOfBreedExistsQueryHandler;
        _anyPetOfSpeciesExistsQueryHandler = anyPetOfSpeciesExistsQueryHandler;
        _createVolunteerCommandHandler = createVolunteerCommandHandler;
    }

    public Task<bool> AnyPetOfBreedExists(
        AnyPetOfBreedExistsRequest request, 
        CancellationToken cancellationToken)
    {
        return _anyPetOfBreedExistsQueryHandler.Handle(request.ToQuery(), cancellationToken);
    }

    public Task<bool> AnyPetOfSpeciesExists(
        AnyPetOfSpeciesExistsRequest request, 
        CancellationToken cancellationToken)
    {
        return _anyPetOfSpeciesExistsQueryHandler.Handle(request.ToQuery(), cancellationToken);
    }

    public Task<Result<Guid, ErrorList>> CreateVolunteer(
        CreateVolunteerRequest request, 
        CancellationToken cancellationToken)
    {
        return _createVolunteerCommandHandler.Handle(request.ToCommand(), cancellationToken);
    }
}
