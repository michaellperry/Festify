using Festify.Indexer.Documents;
using System.Threading.Tasks;

namespace Festify.Indexer.Updaters;

public class VenueUpdater
{
    private readonly IRepository repository;

    public VenueUpdater(IRepository repository)
    {
            this.repository = repository;
        }

    public async Task<VenueDocument> UpdateAndGetLatestVenue(VenueDocument updatedVenue)
    {
            VenueDocument venue = await repository.GetVenue(updatedVenue.VenueGuid);
            bool shouldUpdate = false;
            if (venue == null)
            {
                venue = updatedVenue;
                shouldUpdate = true;
            }
            else
            {
                if (updatedVenue.Description != null &&
                    (venue.Description == null ||
                     venue.Description.ModifiedDate < updatedVenue.Description.ModifiedDate))
                {
                    venue.Description = updatedVenue.Description;
                    shouldUpdate = true;
                }
                if (updatedVenue.Location != null &&
                    (venue.Location == null ||
                     venue.Location.ModifiedDate < updatedVenue.Location.ModifiedDate))
                {
                    venue.Location = updatedVenue.Location;
                    shouldUpdate = true;
                }
            }
            if (shouldUpdate)
            {
                await repository.IndexVenue(venue);
            }

            return venue;
        }
}