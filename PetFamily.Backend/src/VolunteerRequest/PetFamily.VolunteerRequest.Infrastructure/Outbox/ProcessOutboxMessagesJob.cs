using Quartz;

namespace PetFamily.VolunteerRequest.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal class ProcessOutboxMessagesJob : IJob
{
    private readonly ProcessOutboxMessagesService _processOutboxMessagesService;

    public ProcessOutboxMessagesJob(ProcessOutboxMessagesService processOutboxMessagesService)
    {
        _processOutboxMessagesService = processOutboxMessagesService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _processOutboxMessagesService.Execute(context.CancellationToken);
    }
}
