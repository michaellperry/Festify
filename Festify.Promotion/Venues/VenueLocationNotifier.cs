using Festify.Promotion.Data;
using MassTransit;
using System.Threading.Tasks;
using Festify.Promotion.Messages.Venues;

namespace Festify.Promotion.Venues
{
    class VenueLocationNotifier : INotifier<VenueLocation>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public VenueLocationNotifier(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Notify(VenueLocation venueLocation)
        {
            var venueLocationChanged = new VenueLocationChanged
            {
                venueGuid = venueLocation.Venue.VenueGuid,
                location = new VenueLocationRepresentation
                {
                    latitude = venueLocation.Latitude,
                    longitude = venueLocation.Longitude,
                    modifiedDate = venueLocation.ModifiedDate
                }
            };

            await publishEndpoint.Publish(venueLocationChanged);
        }
    }
}