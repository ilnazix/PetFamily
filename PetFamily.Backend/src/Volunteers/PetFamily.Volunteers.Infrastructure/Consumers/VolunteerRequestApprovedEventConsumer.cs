using MassTransit;
using PetFamily.VolunteerRequest.Contracts.Messaging;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;

namespace PetFamily.Volunteers.Infrastructure.Consumers;

internal class VolunteerRequestApprovedEventConsumer 
    : IConsumer<VolunteerRequestApprovedEvent>
{
    private readonly CreateVolunteerCommandHandler _handler;

    public VolunteerRequestApprovedEventConsumer(CreateVolunteerCommandHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<VolunteerRequestApprovedEvent> context)
    {
        var message = context.Message;

        var fullNameDto = new FullNameDto(
            message.FirstName, 
            message.LastName, 
            message.MiddleName);

        var command = new CreateVolunteerCommand(
            FullName: fullNameDto,
            PhoneNumber: message.PhoneNumber,
            Email: message.Email
        );

        await _handler.Handle(command, context.CancellationToken);
    }
}
