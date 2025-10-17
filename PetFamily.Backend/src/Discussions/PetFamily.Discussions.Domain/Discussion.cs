using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Domain;

public class Discussion : Entity<DiscussionId>
{
    private const int MAX_DISCUSSION_PARTICIPANTS = 2;
    public Guid RelationId { get; private set; }
    public IReadOnlyList<Message> Messages => _messages;
    public IReadOnlyList<Guid> ParticipantIds => _participantIds;
    public bool IsClosed { get; private set; }

    private List<Message> _messages = new();
    private List<Guid> _participantIds = new();

    private Discussion(){}
    private Discussion(
        DiscussionId id,
        Guid relationId,
        IEnumerable<Guid> users)
    {
        Id = id; 
        RelationId = relationId;
        IsClosed = false;
        _participantIds = users.ToList();
    }

    public static Result<Discussion, Error> Create(
        DiscussionId id,
        Guid relationId,
        IEnumerable<Guid> participantIds)
    {
        if (relationId == Guid.Empty)
        {
            return Error.Validation(
                code: "discussion.relationId.required",
                message: "RelationId is required to create a discussion.",
                nameof(relationId));
        }

        if (participantIds is null || participantIds.Count() != MAX_DISCUSSION_PARTICIPANTS)
        {
            return Error.Validation(
                code: "discussion.participants.invalidCount",
                message: $"Discussion must contain exactly {MAX_DISCUSSION_PARTICIPANTS} participants.",
                nameof(participantIds));
        }

        return new Discussion(id, relationId, participantIds);
    }

    public UnitResult<Error> AddMessage(Message message)
    {
        var messageAuthorId = message.UserId;

        if (!_participantIds.Any(participantId => participantId == messageAuthorId))
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