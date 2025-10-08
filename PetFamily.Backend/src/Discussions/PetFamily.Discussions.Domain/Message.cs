using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Domain;

public class Message : Entity<MessageId>
{
    public string Text { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsEdited { get; private set; }

    private Message() { }
    private Message(
        MessageId id,
        Guid userId,
        string text)
    {
        Id = id;
        UserId = userId;
        Text = text;
        CreatedAt = DateTime.UtcNow;
        IsEdited = false;
    }

    public static Result<Message, Error> Create(
        MessageId id,
        Guid userId,
        string text)
    {
        if (userId == Guid.Empty)
            return Errors.Message.MessageAuthorRequired();

        if (string.IsNullOrWhiteSpace(text))
            return Errors.Message.MessageTextRequired();

        text = text.Trim();

        return new Message(id, userId, text);
    }


    internal UnitResult<Error> Edit(
        Guid userId,
        string newText) 
    {
        if (string.IsNullOrWhiteSpace(newText))
           return Errors.Message.MessageTextRequired();

        if (userId != UserId)
            return Errors.Message.EditNotAllowed();

        Text = newText;
        IsEdited = true;

        return UnitResult.Success<Error>();
    }
}

