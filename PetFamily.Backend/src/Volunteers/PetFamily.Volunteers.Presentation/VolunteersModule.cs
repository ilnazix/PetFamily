using PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfBreedExists;
using PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfSpeciesExists;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Volunteers.Presentation.Pets.Extensions;
using System.Runtime.CompilerServices;

namespace PetFamily.Volunteers.Presentation
{
    internal class VolunteersModule : IVolunteersModule
    {
        private readonly AnyPetOfBreedExistsQueryHandler _anyPetOfBreedExistsQueryHandler;
        private readonly AnyPetOfSpeciesExistsQueryHandler _anyPetOfSpeciesExistsQueryHandler;

        public VolunteersModule(
            AnyPetOfBreedExistsQueryHandler anyPetOfBreedExistsQueryHandler,
            AnyPetOfSpeciesExistsQueryHandler anyPetOfSpeciesExistsQueryHandler)
        {
            _anyPetOfBreedExistsQueryHandler = anyPetOfBreedExistsQueryHandler;
            _anyPetOfSpeciesExistsQueryHandler = anyPetOfSpeciesExistsQueryHandler;
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
    }


}
