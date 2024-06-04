using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Venues;

public class VenueLocationConfiguration : IEntityTypeConfiguration<VenueLocation>
{
    public void Configure(EntityTypeBuilder<VenueLocation> builder)
    {
        builder.HasAlternateKey(venueLocation => new { venueLocation.VenueId, venueLocation.ModifiedDate });
    }
}