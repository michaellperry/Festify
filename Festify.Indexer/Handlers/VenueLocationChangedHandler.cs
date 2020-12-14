using Festify.Indexer.Documents;
using Festify.Promotion.Messages.Venues;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer.Handlers
{
    public class VenueLocationChangedHandler
    {
        private readonly IRepository repository;

        public VenueLocationChangedHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(VenueLocationChanged venueLocationChanged)
        {
            Console.WriteLine($"Updating index for venue location {venueLocationChanged.location.latitude}, {venueLocationChanged.location.longitude}.");
            try
            {
                string venueGuid = venueLocationChanged.venueGuid.ToString().ToLower();
                VenueLocation venueLocation = VenueLocation.FromRepresentation(venueLocationChanged.location);

                await repository.UpdateShowsWithVenueLocation(venueGuid, venueLocation);
                Console.WriteLine("Succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
