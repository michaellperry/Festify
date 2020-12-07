using System.ComponentModel.DataAnnotations;
using System;
using Festify.Promotion.Messages.Venues;

namespace Festify.Promotion.Venues
{
    public class VenueInfo
    {
        public Guid VenueGuid { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public long LastModifiedTicks { get; set; }
        [Required]
        public string TimeZone { get; set; }
        public long TimeZoneLastModifiedTicks { get; set; }
        [Required]
        public float? Latitude { get; set; }
        [Required]
        public float? Longitude { get; set; }
        public long LocationLastModifiedTicks { get; set; }

        public static VenueInfo FromEntities(
            Guid venueGuid,
            VenueDescription venueDescription,
            VenueLocation venueLocation = null,
            VenueTimeZone venueTimeZone = null)
        {
            return new VenueInfo
            {
                VenueGuid = venueGuid,
                Name = venueDescription?.Name,
                City = venueDescription?.City,
                LastModifiedTicks = venueDescription?.ModifiedDate.Ticks ?? 0,
                TimeZone = venueTimeZone?.TimeZone,
                TimeZoneLastModifiedTicks = venueTimeZone?.ModifiedDate.Ticks ?? 0,
                Latitude = venueLocation?.Latitude,
                Longitude = venueLocation?.Longitude,
                LocationLastModifiedTicks = venueLocation?.ModifiedDate.Ticks ?? 0
            };
        }

        public VenueLocationRepresentation ToVenueLocationRepresentation()
        {
            switch ((Latitude, Longitude))
            {
                case (float latitude, float longitude):
                    return new VenueLocationRepresentation
                    {
                        latitude = latitude,
                        longitude = longitude,
                        modifiedDate = new DateTime(this.LocationLastModifiedTicks)
                    };
                default:
                    return null;
            }
        }
    }
}