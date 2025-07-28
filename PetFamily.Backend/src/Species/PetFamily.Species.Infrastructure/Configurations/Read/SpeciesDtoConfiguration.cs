using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Species.Application.DTOs;

namespace PetFamily.Species.Infrastructure.Configurations.Read
{
    internal class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
    {
        public void Configure(EntityTypeBuilder<SpeciesDto> builder)
        {
            builder.ToTable(Tables.Species);
            builder.HasKey(x => x.Id);
        }
    }
}
