using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Navigation.Domain.Model.Navigation.Domain.Models;
using Navigation.Domain.ValueObjects;

namespace Navigation.Infrastructure.Configurations
{
    public class JourneyPublicLinkConfiguration : IEntityTypeConfiguration<JourneyPublicLink>
    {
        public void Configure(EntityTypeBuilder<JourneyPublicLink> builder)
        {
            builder.HasKey(jpl => jpl.Id);

            builder.Property(jpl => jpl.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => JourneyPublicLinkId.Of(dbValue))
                .IsRequired();

            builder.Property(jpl => jpl.JourneyId)
                .HasConversion(
                    id => id.Value,
                    dbValue => JourneyId.Of(dbValue))
                .IsRequired();

            builder.Property(jpl => jpl.Link)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(jpl => jpl.IsRevoked)
                .IsRequired();

            builder.Property(jpl => jpl.CreatedAt)
                .IsRequired();

            builder.Property(jpl => jpl.RevokedAt);
        }
    }
}
