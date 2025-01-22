using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Application.Volunteers.Restore
{
    public class RestoreVolunteerCommandHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<RestoreVolunteerCommandHandler> _logger;

        public RestoreVolunteerCommandHandler(
            IVolunteersRepository volunteersRepository, 
            ILogger<RestoreVolunteerCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            RestoreVolunteerCommand command, 
            CancellationToken cancellationToken)
        {
            var id = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error;
            }

            var volunteer = volunteerResult.Value;
            volunteer.Restore();

            await _volunteersRepository.Save(volunteer, cancellationToken);

            _logger.LogInformation("Restore volunteer with id={id}", id.Value);

            return id.Value;
        }
    }
}
