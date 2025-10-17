using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequest.Application.DTOs;
using System.Text.Json;

namespace PetFamily.VolunteerRequest.Infrastructure.Configurations.Read;

internal class VolunteerRequestDtoConfiguration : IEntityTypeConfiguration<VolunteerRequestDto>
{
    public void Configure(EntityTypeBuilder<VolunteerRequestDto> builder)
    {
        builder.ToTable(Tables.VolunteerRequests);

        builder.Property(v => v.VolunteerInfo)
            .HasConversion(v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default), 
                s => JsonSerializer.Deserialize<VolunteerInfoDto>(s, JsonSerializerOptions.Default)!);
    }
}
