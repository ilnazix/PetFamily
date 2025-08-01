using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;


namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedias
{
    public class UpdateSocialMediasCommandHandler : ICommandHandler<Guid, UpdateSocialMediasCommand>
    {
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateSocialMediasCommand> _validator;
        private readonly ILogger<UpdateSocialMediasCommandHandler> _logger;

        public UpdateSocialMediasCommandHandler(
            IVolunteersUnitOfWork unitOfWork,
            IValidator<UpdateSocialMediasCommand> validator,
            ILogger<UpdateSocialMediasCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateSocialMediasCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToErrorList();
            }

            var id = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var socialMediaDtos = command.SocialMedias;
            var socialMedias = socialMediaDtos.Select(sm => SocialMedia.Create(sm.Link, sm.Title).Value).ToList();

            volunteerResult.Value.UpdateSocialMedias(socialMedias);

            await _unitOfWork.Commit(cancellationToken);

            _logger.LogInformation("Volunteers's (Id={Id}) social medias list updated", id.Value);

            return id.Value;
        }
    }
}
