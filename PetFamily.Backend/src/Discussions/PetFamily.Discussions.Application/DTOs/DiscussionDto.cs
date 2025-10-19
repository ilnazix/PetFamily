namespace PetFamily.Discussions.Application.DTOs;

public class DiscussionDto
{
    public Guid Id { get; set; }
    public Guid RelationId { get; set; }
    public List<MessageDto> Messages { get; set; } = new();
    public bool IsClosed { get; set; }
}