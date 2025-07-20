using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithFilters
{
    public record GetPetsWithFiltersQueryHandler 
        : IQueryHandler<PagedList<PetDto>, GetPetsWithFiltersQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetsWithFiltersQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetPetsWithFiltersQuery query, 
            CancellationToken cancelationToken = default)
        {
            var petsQuery = _readDbContext.Pets;

            petsQuery = petsQuery.Sort(query.OrderBy);

            var result = await petsQuery.ToPagedList(query.PageNumber, query.PageSize, cancelationToken);

            return result;
        }
    }
}
