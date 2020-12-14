using Festify.Indexer.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Festify.Indexer.UnitTest
{
    class InMemoryRepository : IRepository
    {
        private List<ShowDocument> shows = new List<ShowDocument>();
        private List<VenueDocument> venues = new List<VenueDocument>();
        private List<ActDocument> acts = new List<ActDocument>();

        public ICollection<ShowDocument> Shows => shows;

        public Task<VenueDocument> GetVenue(string venueGuid)
        {
            return Task.FromResult(venues.SingleOrDefault(venue => venue.VenueGuid == venueGuid));
        }

        public Task<ActDocument> GetAct(string actGuid)
        {
            return Task.FromResult(acts.SingleOrDefault(act => act.ActGuid == actGuid));
        }

        public Task IndexVenue(VenueDocument venue)
        {
            venues.RemoveAll(v => v.VenueGuid == venue.VenueGuid);
            venues.Add(DeepCopy(venue));
            return Task.CompletedTask;
        }

        public Task IndexAct(ActDocument act)
        {
            acts.RemoveAll(a => a.ActGuid == act.ActGuid);
            acts.Add(DeepCopy(act));
            return Task.CompletedTask;
        }

        public Task IndexShow(ShowDocument ShowDocument)
        {
            shows.RemoveAll(s =>
                s.ActGuid == ShowDocument.ActGuid &&
                s.VenueGuid == ShowDocument.VenueGuid &&
                s.StartTime == ShowDocument.StartTime);
            shows.Add(DeepCopy(ShowDocument));
            return Task.CompletedTask;
        }

        public Task UpdateShowsWithVenueDescription(string venueGuid, VenueDescription venueDescription)
        {
            foreach (var show in shows.Where(s => s.VenueGuid == venueGuid))
            {
                show.VenueDescription = DeepCopy(venueDescription);
            }
            return Task.CompletedTask;
        }

        public Task UpdateShowsWithVenueLocation(string venueGuid, VenueLocation venueLocation)
        {
            foreach (var show in shows.Where(s => s.VenueGuid == venueGuid))
            {
                show.VenueLocation = DeepCopy(venueLocation);
            }
            return Task.CompletedTask;
        }

        public Task UpdateShowsWithActDescription(string actGuid, ActDescription actDescription)
        {
            foreach (var show in shows.Where(s => s.ActGuid == actGuid))
            {
                show.ActDescription = DeepCopy(actDescription);
            }
            return Task.CompletedTask;
        }

        private static T DeepCopy<T>(T obj)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj));
        }
    }
}