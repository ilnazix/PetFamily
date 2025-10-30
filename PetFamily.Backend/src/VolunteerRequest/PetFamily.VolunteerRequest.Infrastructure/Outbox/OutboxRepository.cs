using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;
using System.Text.Json;

namespace PetFamily.VolunteerRequest.Infrastructure.Outbox;

internal class OutboxRepository : IOutboxRepository
{
    private readonly VolunteerRequestsWriteDbContext _dbContext;
    public OutboxRepository(VolunteerRequestsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add<T>(T message, 
        CancellationToken cancellationToken = default)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message),
            OccurredOnUtc = DateTime.UtcNow
        };

        await _dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}
