namespace PetFamily.Core.Dtos;

public class RequisiteInfo
{
    public RequisiteInfo()
    {
    }
    public RequisiteInfo(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
