using MassTransit;
using PetFamily.Discussions.Application.Commands.CloseDiscussion;
using PetFamily.VolunteerRequest.Contracts.Messaging;

namespace PetFamily.Discussions.Infrastructure.Consumers;

public class VolunteerRequestProcessedEventConsumer : IConsumer<VolunteerRequestProcessedEvent>
{
    private readonly CloseDiscussionByRelationIdCommandHandler _handler;

    public VolunteerRequestProcessedEventConsumer(CloseDiscussionByRelationIdCommandHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<VolunteerRequestProcessedEvent> context)
    {
        var message = context.Message;

        var command = new CloseDiscussionByRelationIdCommand(message.VolunteerRequestId);

        await _handler.Handle(command, context.CancellationToken);
    }
}
