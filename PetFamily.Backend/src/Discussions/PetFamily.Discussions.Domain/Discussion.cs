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

    private readonly List<Message> _messages = new();
    private readonly List<Guid> _participantIds = new();

    private Discussion() { }

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
            return Errors.Discussion.RelationIdRequired();

        if (participantIds is null || participantIds.Count() != MAX_DISCUSSION_PARTICIPANTS)
            return Errors.Discussion.InvalidParticipantCount(MAX_DISCUSSION_PARTICIPANTS);

        return new Discussion(id, relationId, participantIds);
    }

    public UnitResult<Error> AddMessage(Message message)
    {
        if (IsClosed)
            return Errors.Discussion.DiscussionClosed();

        var messageAuthorId = message.UserId;

        if (!_participantIds.Any(p => p == messageAuthorId))
            return Errors.Discussion.MessageAuthorNotParticipant();

        _messages.Add(message);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> EditMessage(Guid userId, MessageId messageId, string newText)
    {
        if (IsClosed)
            return Errors.Discussion.DiscussionClosed();

        var message = _messages.FirstOrDefault(m => m.Id == messageId);

        if (message is null)
            return Errors.General.NotFound();

        return message.Edit(userId, newText);
    }

    public UnitResult<Error> DeleteMessage(Guid userId, MessageId messageId)
    {
        if (IsClosed)
            return Errors.Discussion.DiscussionClosed();

        var message = _messages.FirstOrDefault(m => m.Id == messageId);
        if (message is null)
            return Errors.General.NotFound();

        if (message.UserId != userId)
            return Errors.Discussion.MessageDeleteNotAuthor();

        _messages.Remove(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Close()
    {
        IsClosed = true;
        return UnitResult.Success<Error>();
    }
}
