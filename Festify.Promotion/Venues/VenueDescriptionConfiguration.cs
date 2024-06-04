using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Venues;

public class VenueDescriptionConfiguration : IEntityTypeConfiguration<VenueDescription>
{
    public void Configure(EntityTypeBuilder<VenueDescription> builder)
    {
        builder.HasAlternateKey(venueDescription => new { venueDescription.VenueId, venueDescription.ModifiedDate });
    }
}