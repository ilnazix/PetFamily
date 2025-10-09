using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Domain;

public class Discussion : Entity<DiscussionId>
{
    private const int MAX_DISCUSSION_PARTICIPANTS = 2;
    public Guid RelationId { get; private set; }
    public IReadOnlyList<Message> Messages => _messages;
    public IReadOnlyList<User> Users => _users;
    public bool IsClosed { get; private set; }

    private List<Message> _messages = new();
    private List<User> _users = new();

    private Discussion(){}
    private Discussion(
        DiscussionId id,
        Guid relationId,
        IEnumerable<User> users)
    {
        Id = id; 
        RelationId = relationId;
        IsClosed = false;
        _users = users.ToList();
    }

    public static Result<Discussion, Error> Create(
        DiscussionId id,
        Guid relationId,
        IEnumerable<User> users)
    {
        if (relationId == Guid.Empty)
        {
            return Error.Validation(
                code: "discussion.relationId.required",
                message: "RelationId is required to create a discussion.",
                nameof(relationId));
        }

        if (users is null || users.Count() != MAX_DISCUSSION_PARTICIPANTS)
        {
            return Error.Validation(
                code: "discussion.participants.invalidCount",
                message: $"Discussion must contain exactly {MAX_DISCUSSION_PARTICIPANTS} participants.",
                nameof(users));
        }

        return new Discussion(id, relationId, users);
    }

    public UnitResult<Error> AddMessage(Message message)
    {
        var messageAuthorId = message.UserId;

        if (!_users.Any(u => u.Id == messageAuthorId))
        {
            return Error.Validation(
                code: "discussion.message.author.notParticipant",
                message: "Only participants of the discussion can add messages.",
                null);
        }

        _messages.Add(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Close()
    {
        IsClosed = true;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> EditMessage(
        Guid userId,
        MessageId messageId,
        string newText)
    {
        var message = _messages
            .FirstOrDefault(m => m.Id == messageId);

        if (message is null)
            return Errors.General.NotFound();

        return message.Edit(userId, newText);
    }

    public UnitResult<Error> DeleteMessage(
         Guid userId,
         MessageId messageId)
    {
        var message = _messages
            .FirstOrDefault(m => m.Id == messageId);

        if (message is null)
            return Errors.General.NotFound();

        if (message.UserId != userId)
        {
            return Error.Validation(
                code: "discussion.message.delete.notAuthor",
                message: "Only the author of the message can delete it.",
                nameof(userId));
        }

        _messages.Remove(message);

        return UnitResult.Success<Error>();
    }
}