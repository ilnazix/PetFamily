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
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateRequisitesCommand> _validator;
        private readonly ILogger<UpdateRequisitesCommandHandler> _logger;

        public UpdateRequisitesCommandHandler(
            IVolunteersUnitOfWork unitOfWork,
            IValidator<UpdateRequisitesCommand> validator,
            ILogger<UpdateRequisitesCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
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
            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var requisitesDtos = command.Requisites;
            var requisites = requisitesDtos.Select(r => Requisite.Create(r.Title, r.Description).Value).ToList();

            volunteerResult.Value.UpdateRequisites(requisites);

            await _unitOfWork.Commit(cancellationToken);

            _logger.LogInformation("Volunteers's (Id={Id}) social medias list updated", volunteerId.Value);

            return volunteerId.Value;
        }
    }
}
