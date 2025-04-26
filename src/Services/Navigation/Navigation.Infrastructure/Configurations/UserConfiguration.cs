using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Navigation.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.Domain.ValueObjects;
using Navigation.Domain.Enums;

namespace Navigation.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => UserId.Of(dbValue))
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.DisplayName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Status)
                .HasConversion(
                    status => status.ToString(),
                    dbStatus => Enum.Parse<UserStatus>(dbStatus))
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired();
        }
    }
}
