using System.Threading.Tasks;
using Festify.Promotion.Messages;
using Festify.Search.Index;

namespace Festify.Search
{
    public class ShowDescriptionHandler
    {
        private readonly ShowSearchIndex index;

        public async Task Handle(ShowDescriptionEvent showDescription)
        {
            var show = await index.GetShow(showDescription.ShowGuid);
            if (show == null)
            {
                show = new ShowRecord();
            }

            if (show.ModifiedDate < showDescription.ModifiedDate)
            {
                show.ModifiedDate = showDescription.ModifiedDate;
                show.Title = showDescription.Title;
                show.Date = showDescription.Date;
                show.City = showDescription.City;
                show.Venue = showDescription.Venue;
                show.ImageHash = showDescription.ImageHash;
                await index.InsertOrUpdateShow(show);
            }
        }
    }
}
