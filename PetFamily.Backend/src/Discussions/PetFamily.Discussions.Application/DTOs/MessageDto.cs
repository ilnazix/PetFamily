namespace PetFamily.Discussions.Application.DTOs;

public class MessageDto 
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public bool IsEdited { get; set; }
}

