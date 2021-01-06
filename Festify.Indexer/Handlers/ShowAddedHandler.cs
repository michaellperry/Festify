using Festify.Indexer.Documents;
using Festify.Indexer.Updaters;
using Festify.Promotion.Messages.Shows;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer.Handlers
{
    public class ShowAddedHandler
    {
        private readonly IRepository repository;
        private readonly ActUpdater actUpdater;
        private readonly VenueUpdater venueUpdater;

        public ShowAddedHandler(IRepository repository, ActUpdater actUpdater, VenueUpdater venueUpdater)
        {
            this.repository = repository;
            this.actUpdater = actUpdater;
            this.venueUpdater = venueUpdater;
        }

        public async Task Handle(ShowAdded showAdded)
        {
            Console.WriteLine($"Indexing a show for {showAdded.act.description.title} at {showAdded.venue.description.name}.");
            try
            {
                string actGuid = showAdded.act.actGuid.ToString().ToLower();
                string venueGuid = showAdded.venue.venueGuid.ToString().ToLower();

                ActDescription actDescription = ActDescription.FromRepresentation(showAdded.act.description);
                VenueDescription venueDescription = VenueDescription.FromRepresentation(showAdded.venue.description);
                VenueLocation venueLocation = VenueLocation.FromRepresentation(showAdded.venue.location);

                ActDocument act = await actUpdater.UpdateAndGetLatestAct(new ActDocument
                {
                    ActGuid = actGuid,
                    Description = actDescription
                });
                VenueDocument venue = await venueUpdater.UpdateAndGetLatestVenue(new VenueDocument
                {
                    VenueGuid = venueGuid,
                    Description = venueDescription,
                    Location = venueLocation
                });
                var show = new ShowDocument
                {
                    ActGuid = actGuid,
                    VenueGuid = venueGuid,
                    StartTime = showAdded.show.startTime,
                    ActDescription = act.Description,
                    VenueDescription = venue.Description,
                    VenueLocation = venue.Location
                };
                await repository.IndexShow(show);
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