using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write;

internal class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable(Tables.ADMIN_ACCOUNTS);

        builder.ComplexProperty(a => a.FullName, fnb =>
        {
            fnb.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(FullName.NAME_MAX_LENGTH)
                .HasColumnName("first_name");

            fnb.Property(fn => fn.LastName)
                .IsRequired()
                .HasMaxLength(FullName.NAME_MAX_LENGTH)
                .HasColumnName("last_name");

            fnb.Property(fn => fn.MiddleName)
                .IsRequired()
                .HasMaxLength(FullName.NAME_MAX_LENGTH)
                .HasColumnName("middle_name");
        });
    }
}
