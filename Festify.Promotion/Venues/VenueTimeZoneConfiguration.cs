using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Venues;

public class VenueTimeZoneConfiguration : IEntityTypeConfiguration<VenueTimeZone>
{
    public void Configure(EntityTypeBuilder<VenueTimeZone> builder)
    {
        builder.HasAlternateKey(venueTimeZone => new { venueTimeZone.VenueId, venueTimeZone.ModifiedDate });
    }
}