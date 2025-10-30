using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteerRequest.Application.Messaging;
using PetFamily.VolunteerRequest.Contracts;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;
using Polly;
using Polly.Registry;
using System.Text.Json;

namespace PetFamily.VolunteerRequest.Infrastructure.Outbox;

internal class ProcessOutboxMessagesService
{
    private const int MAX_DEGREE_OF_PARALLELISM = 5;
    private const int BATCH_SIZE = 20;

    private readonly VolunteerRequestsWriteDbContext _dbContext;
    private readonly ResiliencePipeline _resiliencePipeline;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<ProcessOutboxMessagesService> _logger;

    public ProcessOutboxMessagesService(
        VolunteerRequestsWriteDbContext dbContext,
        Bind<IVolunteerRequestsBus, IPublishEndpoint> publisher,
        ResiliencePipelineProvider<string> resiliencePipelineProvider,
        ILogger<ProcessOutboxMessagesService> logger)
    {
        _dbContext = dbContext;
        _resiliencePipeline = resiliencePipelineProvider.GetPipeline(Constants.Resilience.BasicPipeline);
        _publisher = publisher.Value;
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(BATCH_SIZE)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
            return;

        var semaphore = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

        var tasks = messages.Select(async message =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                await ProcessMessage(message, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessMessage(OutboxMessage message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var messageType = AssemblyReference.Assembly.GetType(message.Type);
        if (messageType == null)
        {
            message.Error = "Message type not found";
            _logger.LogError("OutboxMessage {MessageId}: Type {MessageType} not found", message.Id, message.Type);
            return;
        }

        var deserializedMessage = JsonSerializer.Deserialize(message.Payload, messageType);
        if (deserializedMessage == null)
        {
            message.Error = "Message payload could not be deserialized";
            _logger.LogError("OutboxMessage {MessageId}: Failed to deserialize payload", message.Id);
            return;
        }

        try
        {
            await _resiliencePipeline.Execute(async token =>
            {
                await _publisher.Publish(deserializedMessage, token);
            }, cancellationToken);

            message.ProcessedOnUtc = DateTime.UtcNow;
            message.Error = null;
            _logger.LogInformation("Successfully processed OutboxMessage {MessageId} of type {MessageType}", message.Id, message.Type);
        }
        catch (Exception ex)
        {
            message.Error = ex.Message;
            _logger.LogError(ex, "Failed to process OutboxMessage {MessageId} of type {MessageType}", message.Id, message.Type);
        }
    }
}
