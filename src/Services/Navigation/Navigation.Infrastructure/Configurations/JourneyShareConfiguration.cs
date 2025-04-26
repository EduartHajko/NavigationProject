using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Navigation.Domain.Model.Navigation.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.Domain.ValueObjects;

namespace Navigation.Infrastructure.Configurations
{
    public class JourneyShareConfiguration : IEntityTypeConfiguration<JourneyShare>
    {
        public void Configure(EntityTypeBuilder<JourneyShare> builder)
        {
            builder.HasKey(js => js.Id);

            builder.Property(js => js.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => JourneyShareId.Of(dbValue))
                .IsRequired();

            builder.Property(js => js.JourneyId)
                .HasConversion(
                    id => id.Value,
                    dbValue => JourneyId.Of(dbValue))
                .IsRequired();

            builder.Property(js => js.SharedWithUserId)
                .IsRequired();

            builder.Property(js => js.SharedAt)
                .IsRequired();
        }
    }
}
