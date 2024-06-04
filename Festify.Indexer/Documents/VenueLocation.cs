using Festify.Promotion.Messages.Venues;
using System;

namespace Festify.Indexer.Documents;

public class VenueLocation
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public DateTime ModifiedDate { get; set; }

    public static VenueLocation FromRepresentation(VenueLocationRepresentation location)
    {
            return new VenueLocation
            {
                Latitude = location.latitude,
                Longitude = location.longitude,
                ModifiedDate = location.modifiedDate
            };
        }
}