using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.VolunteerRequest.Domain;

namespace PetFamily.VolunteerRequest.Infrastructure.Configurations.Write;

internal class VolunteerRequestConfiguration : IEntityTypeConfiguration<Domain.VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<Domain.VolunteerRequest> builder)
    {
        builder.ToTable(Tables.VolunteerRequests);

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(id => id.Value,
                v => VolunteerRequestId.Create(v));

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion(s => s.Value,
                v => VolunteerRequestStatus.Create(v).Value);

        builder.Property(t => t.CreatedAt).IsRequired();

        builder.OwnsOne(t => t.VolunteerInfo, vib =>
        {
            vib.ToJson();

            vib.Property(vi => vi.Email)
                .HasConversion(e => e.Value,
                    v => Email.Create(v).Value);

            vib.Property(vi => vi.PhoneNumber)
                .HasConversion(p => p.Value,
                    v => PhoneNumber.Create(v).Value);

            vib.OwnsOne(vi => vi.FullName, flb =>
            {
                flb.Property(flb => flb.FirstName).HasColumnName("first_name");

                flb.Property(flb => flb.LastName).HasColumnName("last_name");

                flb.Property(flb => flb.MiddleName).HasColumnName("middle_name");
            });
        });
    }
}
