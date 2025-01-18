using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolunteersRepository volunteersRepository, 
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateMainInfoCommand command, 
            CancellationToken cancellationToken = default)
        {
            var id = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error;
            }

            var (firstName, lastName, middleName) = command.Dto.FullName;
            var fullName = FullName.Create(firstName, lastName, middleName).Value;

            var description = Description.Create(command.Dto.Description).Value;
            var phoneNumber = PhoneNumber.Create(command.Dto.PhoneNumber).Value;
            var email = Email.Create(command.Dto.Email).Value;
            var experience = Experience.Create(command.Dto.Experience).Value;

            volunteerResult.Value.UpdateMainInfo(fullName, description, email, phoneNumber, experience);

            await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteer's (id={id}) main info updated", volunteerResult.Value.Id);

            return id.Value;
        }
    }
}
