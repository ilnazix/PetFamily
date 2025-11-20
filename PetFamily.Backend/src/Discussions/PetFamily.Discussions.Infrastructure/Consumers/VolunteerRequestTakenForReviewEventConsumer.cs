using MassTransit;
using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.VolunteerRequest.Contracts.Messaging;

namespace PetFamily.Discussions.Infrastructure.Consumers;

public class VolunteerRequestTakenForReviewEventConsumer 
    : IConsumer<VolunteerRequestTakenForReviewEvent>
{
    private readonly CreateDiscussionCommandHandler _handler;

    public VolunteerRequestTakenForReviewEventConsumer(
        CreateDiscussionCommandHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<VolunteerRequestTakenForReviewEvent> context)
    {
        var message = context.Message;
        
        var participantIds = new List<Guid>
        {
            message.UserId,
            message.AdminId
        };  

        var command = new CreateDiscussionCommand
        (
           RelationId: message.VolunteerRequestId,
           ParticipantIds: participantIds
        );

        await _handler.Handle(command, context.CancellationToken);
    }
}
