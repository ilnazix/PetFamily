using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Species.Queries.GetFilteredSpeciesWithPagination
{
    public class GetFilteredSpeciesWithPaginationQueryHandler 
        : IQueryHandler<PagedList<SpeciesDto>, GetFilteredSpeciesWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetFilteredSpeciesWithPaginationQueryHandler(
            IReadDbContext readDbContext)
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
