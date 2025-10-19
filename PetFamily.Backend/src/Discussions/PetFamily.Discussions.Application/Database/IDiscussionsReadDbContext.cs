using PetFamily.Discussions.Application.DTOs;

namespace PetFamily.Discussions.Application.Database;

public interface IDiscussionsReadDbContext 
{
    IQueryable<DiscussionDto> Discussions { get; }
}
