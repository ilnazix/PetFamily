using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Application.Volunteers.HardDelete
{
    public class HardDeleteCommandHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<HardDeleteCommandHandler> _logger;

        public HardDeleteCommandHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<HardDeleteCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            HardDeleteCommand command, 
            CancellationToken cancellationToken)
        {
            var id = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error;
            }

            await _volunteersRepository.Delete(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteer with id={id} permanently deleted", id.Value);

            return id.Value;
        }
    }
}
