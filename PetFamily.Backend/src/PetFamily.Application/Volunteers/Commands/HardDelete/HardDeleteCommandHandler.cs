using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.HardDelete
{
    public class HardDeleteCommandHandler : ICommandHandler<Guid, HardDeleteCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<HardDeleteCommand> _validator;
        private readonly ILogger<HardDeleteCommandHandler> _logger;

        public HardDeleteCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<HardDeleteCommand> validator,
            ILogger<HardDeleteCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            HardDeleteCommand command,
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

            _logger.LogInformation("Volunteers with id={id} permanently deleted", id.Value);

            return id.Value;
        }
    }
}
