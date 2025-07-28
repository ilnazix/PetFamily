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
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<UpdateSocialMediasCommand> _validator;
        private readonly ILogger<UpdateSocialMediasCommandHandler> _logger;

        public UpdateSocialMediasCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<UpdateSocialMediasCommand> validator,
            ILogger<UpdateSocialMediasCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
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
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var socialMediaDtos = command.SocialMedias;
            var socialMedias = socialMediaDtos.Select(sm => SocialMedia.Create(sm.Link, sm.Title).Value).ToList();

            volunteerResult.Value.UpdateSocialMedias(socialMedias);

            var guid = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteers's (Id={Id}) social medias list updated", guid.Value);

            return guid.Value;
        }
    }
}
