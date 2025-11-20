using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.DTOs;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithFilters;

public record GetPetsWithFiltersQueryHandler
    : IQueryHandler<PagedList<PetDto>, GetPetsWithFiltersQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public GetPetsWithFiltersQueryHandler(IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<PetDto>> Handle(
GetPetsWithFiltersQuery query,
CancellationToken cancellationToken = default)
    {
        var petsQuery = _readDbContext.Pets.AsQueryable();

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            p => p.Name.Contains(query.Name!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Description),
            p => p.Description.Contains(query.Description!));

        petsQuery = petsQuery.WhereIf(
            query.VolunteerIds is not null && query.VolunteerIds.Any(),
            p => query.VolunteerIds!.Contains(p.VolunteerId));

        petsQuery = petsQuery.WhereIf(
            query.SpeciesId is not null,
            p => p.SpeciesId == query.SpeciesId);

        petsQuery = petsQuery.WhereIf(
            query.BreedId is not null,
            p => p.BreedId == query.BreedId);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Color),
            p => p.Color == query.Color);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Status),
            p => p.Status == query.Status);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Country),
            p => p.Country == query.Country);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.State),
            p => p.State == query.State);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.City),
            p => p.City == query.City);

        petsQuery = petsQuery.Sort(query.OrderBy);

        var result = await petsQuery.ToPagedList(query.PageNumber, query.PageSize, cancellationToken);

        return result;
    }
}
