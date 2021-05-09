using Autofac;
using Festify.Indexer.Consumers;
using Festify.Indexer.Updaters;

namespace Festify.Indexer
{
    public class IndexerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ActUpdater>()
                .AsSelf();
            builder
                .RegisterType<VenueUpdater>()
                .AsSelf();
            builder
                .RegisterType<ShowAddedConsumer>()
                .AsImplementedInterfaces();
            builder
                .RegisterType<VenueDescriptionChangedConsumer>()
                .AsImplementedInterfaces();
            builder
                .RegisterType<VenueLocationChangedConsumer>()
                .AsImplementedInterfaces();
        }
    }
}
