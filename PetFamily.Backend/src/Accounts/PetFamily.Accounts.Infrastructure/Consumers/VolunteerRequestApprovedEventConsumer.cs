using MassTransit;
using PetFamily.VolunteerRequest.Contracts.Messaging;
using PetFamily.Accounts.Application.Commands.GrantVolunteerRole;

namespace PetFamily.Accounts.Infrastructure.Consumers;

internal class VolunteerRequestApprovedEventConsumer : IConsumer<VolunteerRequestApprovedEvent>
{
    private readonly GrantVolunteerRoleCommandHandler _handler;

    public VolunteerRequestApprovedEventConsumer(GrantVolunteerRoleCommandHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<VolunteerRequestApprovedEvent> context)
    {
        var command = new GrantVolunteerRoleCommand(
            UserId: context.Message.UserId
        );

        await _handler.Handle(command, context.CancellationToken);
    }
}
