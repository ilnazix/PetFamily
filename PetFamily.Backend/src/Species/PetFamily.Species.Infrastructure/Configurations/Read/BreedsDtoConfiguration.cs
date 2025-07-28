using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Species.Application.DTOs;

namespace PetFamily.Species.Infrastructure.Configurations.Read
{
    internal class BreedsDtoConfiguration : IEntityTypeConfiguration<BreedDto>
    {
        public void Configure(EntityTypeBuilder<BreedDto> builder)
        {
            builder.ToTable(Tables.Breeds);

            builder.HasKey(b => b.Id);
        }
    }
}
