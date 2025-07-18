using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.DeletePermanently
{
    public class DeleteVolunteerPermanentlyCommandHandler : ICommandHandler<Guid, DeleteVolunteerPermanentlyCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<DeleteVolunteerPermanentlyCommand> _validator;
        private readonly ILogger<DeleteVolunteerPermanentlyCommandHandler> _logger;

        public DeleteVolunteerPermanentlyCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<DeleteVolunteerPermanentlyCommand> validator,
            ILogger<DeleteVolunteerPermanentlyCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            DeleteVolunteerPermanentlyCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToErrorList();
            }

            var id = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            await _volunteersRepository.Delete(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteers with Id={Id} permanently deleted", id.Value);

            return id.Value;
        }
    }
}
