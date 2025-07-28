using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites
{
    public class UpdateRequisitesCommandHandler : ICommandHandler<Guid, UpdateRequisitesCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<UpdateRequisitesCommand> _validator;
        private readonly ILogger<UpdateRequisitesCommandHandler> _logger;

        public UpdateRequisitesCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<UpdateRequisitesCommand> validator,
            ILogger<UpdateRequisitesCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }


        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateRequisitesCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToErrorList();
            }

            var volunteerId = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var requisitesDtos = command.Requisites;
            var requisites = requisitesDtos.Select(r => Requisite.Create(r.Title, r.Description).Value).ToList();

            volunteerResult.Value.UpdateRequisites(requisites);

            var guid = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteers's (Id={Id}) social medias list updated", guid.Value);

            return guid.Value;
        }
    }
}
