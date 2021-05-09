using Festify.Indexer.Documents;
using Festify.Indexer.Updaters;
using Festify.Promotion.Messages.Acts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer.Consumers
{
    public class ActDescriptionChangedConsumer : IConsumer<ActDescriptionChanged>
    {
        private readonly IRepository repository;
        private readonly ActUpdater actUpdater;

        public ActDescriptionChangedConsumer(IRepository repository, ActUpdater actUpdater)
        {
            this.repository = repository;
            this.actUpdater = actUpdater;
        }

        public async Task Consume(ConsumeContext<ActDescriptionChanged> context)
        {
            var actDescriptionChanged = context.Message;
            Console.WriteLine($"Updating index for act {actDescriptionChanged.description.title}.");
            try
            {
                string actGuid = actDescriptionChanged.actGuid.ToString().ToLower();
                ActDescription actDescription = ActDescription.FromRepresentation(actDescriptionChanged.description);
                ActDocument act = await actUpdater.UpdateAndGetLatestAct(new ActDocument
                {
                    ActGuid = actGuid,
                    Description = actDescription
                });
                await repository.UpdateShowsWithActDescription(actGuid, act.Description);
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
