using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.DTOs;
using System.Text.Json;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read
{
    internal class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable(Tables.Pets);

            builder.HasKey(p => p.Id);

            builder.Property(v => v.Requisites)
                .HasConversion(req => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                 json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);


            builder.Property(v => v.Photos)
                .HasConversion(req => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                 json => JsonSerializer.Deserialize<PhotoDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
