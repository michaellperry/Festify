using Festify.Indexer.Documents;
using System.Threading.Tasks;

namespace Festify.Indexer.Updaters
{
    public class ActUpdater
    {
        private readonly IRepository repository;

        public ActUpdater(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ActDocument> UpdateAndGetLatestAct(ActDocument updatedAct)
        {
            var act = await repository.GetAct(updatedAct.ActGuid);
            bool shouldUpdate = false;
            if (act == null)
            {
                act = updatedAct;
                shouldUpdate = true;
            }
            else
            {
                if (
                    updatedAct.Description != null &&
                    (act.Description == null ||
                     act.Description.ModifiedDate < updatedAct.Description.ModifiedDate))
                {
                    act.Description = updatedAct.Description;
                    shouldUpdate = true;
                }
            }
            if (shouldUpdate)
            {
                await repository.IndexAct(act);
            }

            return act;
        }
    }
}