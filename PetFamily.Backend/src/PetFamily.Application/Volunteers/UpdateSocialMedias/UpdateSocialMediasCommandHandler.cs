using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateSocialMedias
{
    public class UpdateSocialMediasCommandHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdateSocialMediasCommandHandler> _logger;

        public UpdateSocialMediasCommandHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<UpdateSocialMediasCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateSocialMediasCommand command,
            CancellationToken cancellationToken = default)
        {
            var id = VolunteerId.Create(command.VolunteerId);
            var volunteerResult =  await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error;
            }

            var socialMediaDtos = command.Dto.SocialMedias;
            var socialMedias = socialMediaDtos.Select(sm => SocialMedia.Create(sm.Link, sm.Title).Value).ToList();

            volunteerResult.Value.UpdateSocialMedias(socialMedias);

            var guid = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteers's (id={id}) social medias list updated", guid.Value);

            return guid;
        }
    }
}
