using Festify.Promotion.Messages.Acts;
using Festify.Promotion.Messages.Shows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Festify.Indexer.UnitTest
{
    class InMemoryRepository : IRepository
    {
        private IList<ShowAdded> shows = new List<ShowAdded>();

        public ICollection<ShowAdded> Shows => shows;

        public Task IndexShow(ShowAdded showAdded)
        {
            shows.Add(DeepCopy(showAdded));
            return Task.CompletedTask;
        }

        public Task UpdateShowsWithActDescription(Guid actGuid, ActDescriptionRepresentation description)
        {
            foreach (var show in shows.Where(s => s.act.actGuid == actGuid))
            {
                show.act.description = DeepCopy(description);
            }
            return Task.CompletedTask;
        }

        private static T DeepCopy<T>(T obj)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj));
        }
    }
}