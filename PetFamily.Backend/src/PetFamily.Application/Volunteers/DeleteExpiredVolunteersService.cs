using Microsoft.Extensions.Options;

namespace PetFamily.Application.Volunteers
{
    public class DeleteExpiredVolunteersService
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly VolunteerEntityOptions _options;

        public DeleteExpiredVolunteersService(IVolunteersRepository volunteersRepository, 
            IOptions<VolunteerEntityOptions> options)
        {
            _volunteersRepository = volunteersRepository;
            _options = options.Value;
        }

        public async Task Process()
        {
            var olderThan = TimeSpan.FromDays(_options.DeleteExpiredVolunteersServiceReductionDays);
            await _volunteersRepository.DeleteExpiredSoftDeletions(olderThan);
        }
    }
}
