using Festify.Indexer.Documents;
using Festify.Promotion.Messages.Shows;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer.Handlers
{
    public class ShowAddedHandler
    {
        private readonly IRepository repository;

        public ShowAddedHandler(IRepository repository)
        {
            this.repository = repository;
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

                var show = new ShowDocument
                {
                    ActGuid = actGuid,
                    VenueGuid = venueGuid,
                    StartTime = showAdded.show.startTime,
                    ActDescription = actDescription,
                    VenueDescription = venueDescription,
                    VenueLocation = venueLocation
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