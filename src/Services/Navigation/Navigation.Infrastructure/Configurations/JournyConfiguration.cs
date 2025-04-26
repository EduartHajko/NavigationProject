using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Navigation.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.Domain.Enums;
using Navigation.Domain.ValueObjects;

namespace Navigation.Infrastructure.Configurations
{
    public class JourneyConfiguration : IEntityTypeConfiguration<Journey>
    {
        public void Configure(EntityTypeBuilder<Journey> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => JourneyId.Of(dbValue))
                .IsRequired();

            builder.Property(j => j.UserId)
                .IsRequired();

            builder.OwnsOne(j => j.StartLocation, startBuilder =>
            {
                startBuilder.Property(l => l.Latitude)
                    .HasColumnName("StartLatitude")
                    .IsRequired();

                startBuilder.Property(l => l.Longitude)
                    .HasColumnName("StartLongitude")
                    .IsRequired();

                startBuilder.Property(l => l.Name)
                    .HasColumnName("StartLocationName")
                    .HasMaxLength(200); // optional: max length
            });

            builder.OwnsOne(j => j.EndLocation, endBuilder =>
            {
                endBuilder.Property(l => l.Latitude)
                    .HasColumnName("EndLatitude")
                    .IsRequired();

                endBuilder.Property(l => l.Longitude)
                    .HasColumnName("EndLongitude")
                    .IsRequired();

                endBuilder.Property(l => l.Name)
                    .HasColumnName("EndLocationName")
                    .HasMaxLength(200); // optional: max length
            });

            builder.Property(j => j.StartTime)
                .IsRequired();

            builder.Property(j => j.EndTime)
                .IsRequired();

            builder.Property(j => j.TransportType)
                .HasConversion(
                    t => t.ToString(),
                    t => (TransportType)Enum.Parse(typeof(TransportType), t))
                .IsRequired();

            builder.Property(j => j.DistanceInKilometers)
                .IsRequired();

            builder.Property(j => j.DailyGoalTriggered)
                .IsRequired();

            builder.HasMany(j => j.SharedWithUsers)
                .WithOne()
                .HasForeignKey(js => js.JourneyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.PublicShareLinks)
                .WithOne()
                .HasForeignKey(jpl => jpl.JourneyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
