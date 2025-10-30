using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public class Color : ValueObject
{
    public const int MAX_COLOR_TITLE_LENGTH = 50;
    public string Title { get; }

    private Color(string title)
    {
        Title = title;
    }

    public static Result<Color, Error> Create(string colorTitle)
    {
        if (string.IsNullOrWhiteSpace(colorTitle) || colorTitle.Length > MAX_COLOR_TITLE_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(colorTitle));
        }

        colorTitle = colorTitle.Trim().ToLower();

        return new Color(colorTitle);
    }

    public static Color DefaultColor => new Color("default");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
    }
}