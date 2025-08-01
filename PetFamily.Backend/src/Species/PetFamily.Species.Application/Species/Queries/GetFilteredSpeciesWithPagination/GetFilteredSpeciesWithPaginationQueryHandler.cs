using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.DTOs;
using PetFamily.Core.Extensions;

namespace PetFamily.Species.Application.Species.Queries.GetFilteredSpeciesWithPagination
{
    public class GetFilteredSpeciesWithPaginationQueryHandler
        : IQueryHandler<PagedList<SpeciesDto>, GetFilteredSpeciesWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetFilteredSpeciesWithPaginationQueryHandler(
            ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public Task<PagedList<SpeciesDto>> Handle(
            GetFilteredSpeciesWithPaginationQuery query,
            CancellationToken cancelationToken = default)
        {
            var species = _readDbContext.Species;

            species = species.WhereIf(!string.IsNullOrEmpty(query.Title),
                s => s.Title.ToLower().Contains(query.Title!.ToLower()));

            var result = species.ToPagedList(query.Page, query.PageSize, cancelationToken);

            return result;
        }
    }
}
