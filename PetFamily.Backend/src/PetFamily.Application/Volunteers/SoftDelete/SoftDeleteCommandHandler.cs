using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Application.Volunteers.SoftDelete
{
    public class SoftDeleteCommandHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<SoftDeleteCommandHandler> _logger;

        public SoftDeleteCommandHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<SoftDeleteCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(SoftDeleteCommand command, CancellationToken cancellationToken)
        {
            var id = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error;
            }

            var volunteer = volunteerResult.Value;
            volunteer.Delete();

            await _volunteersRepository.Save(volunteer, cancellationToken);

            _logger.LogInformation("Soft delete volunteer with id={id}", id.Value);

            return id.Value;
        }
    }
}
