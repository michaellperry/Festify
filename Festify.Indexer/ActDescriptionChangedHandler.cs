using Festify.Promotion.Messages.Acts;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer
{
    public class ActDescriptionChangedHandler
    {
        private readonly IRepository repository;

        public ActDescriptionChangedHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(ActDescriptionChanged actDescriptionChanged)
        {
            Console.WriteLine($"Updating index for act {actDescriptionChanged.description.title}.");
            try
            {
                await repository.UpdateShowsWithActDescription(actDescriptionChanged.actGuid, actDescriptionChanged.description);
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
