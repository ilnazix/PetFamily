using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.DTOs;
using PetFamily.Core.Extensions;

namespace PetFamily.Species.Application.Species.Queries.GetFilteredBreedsWithPagination
{
    public class GetFilteredBreedsWithPaginationQueryHandler
        : IQueryHandler<PagedList<BreedDto>, GetFilteredBreedsWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetFilteredBreedsWithPaginationQueryHandler(
            ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<BreedDto>> Handle(GetFilteredBreedsWithPaginationQuery query, CancellationToken cancelationToken = default)
        {
            var breeds = _readDbContext.Breeds.Where(b => b.SpeciesId == query.SpeciesId);

            breeds = breeds.WhereIf(!string.IsNullOrEmpty(query.Title),
                            b => b.Title.ToLower().Contains(query.Title!.ToLower()));

            var result = await breeds.ToPagedList(query.Page, query.PageSize, cancelationToken);
            return result;
        }
    }

}
