using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.SoftDelete
{
    public class SoftDeleteCommandHandler : ICommandHandler<Guid, SoftDeleteCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<SoftDeleteCommand> _validator;
        private readonly ILogger<SoftDeleteCommandHandler> _logger;

        public SoftDeleteCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<SoftDeleteCommand> validator,
            ILogger<SoftDeleteCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(SoftDeleteCommand command, CancellationToken cancellationToken)
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

            var volunteer = volunteerResult.Value;
            volunteer.Delete();

            await _volunteersRepository.Save(volunteer, cancellationToken);

            _logger.LogInformation("Soft delete volunteer with id={id}", id.Value);

            return id.Value;
        }
    }
}
