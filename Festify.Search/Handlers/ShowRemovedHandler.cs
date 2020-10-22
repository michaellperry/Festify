using System;
using System.Threading.Tasks;
using Festify.Promotion.Messages;
using Festify.Search.Index;

namespace Festify.Search
{
    public class ShowRemovedHandler
    {
        private readonly ShowSearchIndex index;

        public async Task Handle(ShowRemovedEvent showRemoved)
        {
            var show = await index.GetShow(showRemoved.ShowGuid);
            if (show != null)
            {
                await index.RemoveShow(showRemoved.ShowGuid);
            }
        }
    }
}
