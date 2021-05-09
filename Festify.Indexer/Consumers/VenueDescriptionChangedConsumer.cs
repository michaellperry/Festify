using Festify.Indexer.Documents;
using Festify.Indexer.Updaters;
using Festify.Promotion.Messages.Venues;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer.Consumers
{
    public class VenueDescriptionChangedConsumer : IConsumer<VenueDescriptionChanged>
    {
        private readonly IRepository repository;
        private readonly VenueUpdater venueUpdater;

        public VenueDescriptionChangedConsumer(IRepository repository, VenueUpdater venueUpdater)
        {
            this.repository = repository;
            this.venueUpdater = venueUpdater;
        }

        public async Task Consume(ConsumeContext<VenueDescriptionChanged> context)
        {
            var venueDescriptionChanged = context.Message;
            Console.WriteLine($"Updating index for venue {venueDescriptionChanged.description.name}.");
            try
            {
                string venueGuid = venueDescriptionChanged.venueGuid.ToString().ToLower();
                VenueDescription venueDescription = VenueDescription.FromRepresentation(venueDescriptionChanged.description);
                VenueDocument updatedVenue = new VenueDocument
                {
                    VenueGuid = venueGuid,
                    Description = venueDescription
                };
                VenueDocument venue = await venueUpdater.UpdateAndGetLatestVenue(updatedVenue);
                await repository.UpdateShowsWithVenueDescription(venueGuid, venue.Description);
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
