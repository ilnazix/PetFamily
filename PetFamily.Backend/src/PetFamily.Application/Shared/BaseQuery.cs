using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Shared
{
    public record BaseQuery(
        int PageNumber, 
        int PageSize) : IQuery;
}
