using Autofac;
using Festify.Indexer.Consumers;
using Festify.Promotion.Messages.Acts;
using Festify.Promotion.Messages.Shows;
using Festify.Promotion.Messages.Venues;
using FluentAssertions;
using MassTransit.AutofacIntegration;
using MassTransit.Testing;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Festify.Indexer.UnitTest
{
    public class HandlerTests : IAsyncDisposable
    {
        private readonly InMemoryRepository repository;
        private readonly IContainer container;
        private readonly InMemoryTestHarness harness;

        private readonly Guid actGuid = Guid.NewGuid();
        private readonly Guid venueGuid = Guid.NewGuid();

        public HandlerTests()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<InMemoryRepository>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterModule<IndexerModule>();
            builder.AddMassTransitInMemoryTestHarness(busConfig =>
            {
                busConfig.AddConsumer<ShowAddedConsumer>();
                busConfig.AddConsumer<ActDescriptionChangedConsumer>();
                busConfig.AddConsumer<VenueDescriptionChangedConsumer>();
                busConfig.AddConsumer<VenueLocationChangedConsumer>();
            });

            container = builder.Build();

            harness = container.Resolve<InMemoryTestHarness>();
            repository = container.Resolve<InMemoryRepository>();

            harness.Start();
        }

        public async ValueTask DisposeAsync()
        {
            await harness.Stop();
            container.Dispose();
        }

        [Fact]
        public async Task WhenShowIsAdded_ShowIsInIndex()
        {
            var showAdded = GivenShowAdded(
                actTitle: "Expected act title",
                venueName: "Expected venue name");
            await WhenPublish(showAdded);

            await harness.Stop();

            repository.Shows.Single().ActDescription.Title.Should().Be("Expected act title");
            repository.Shows.Single().VenueDescription.Name.Should().Be("Expected venue name");
        }

        [Fact]
        public async Task WhenActDescriptionIsChangedAfterShowIsAdded_ThenShowIsUpdated()
        {
            var showAdded = GivenShowAdded(actTitle: "Original Title", actDescriptionAge: 1);
            var actDescriptionChanged = GivenActDescriptionChanged(actTitle: "Modified Title");

            await WhenPublish(showAdded);
            await WhenPublish(actDescriptionChanged);

            await harness.Stop();

            repository.Shows.Single().ActDescription.Title.Should().Be("Modified Title");
        }

        [Fact]
        public async Task WhenActDescriptionChangeArrivesBeforeShowAdded_ThenShowUsesLatestDecsription()
        {
            var showAdded = GivenShowAdded(actTitle: "Original Title", actDescriptionAge: 1);
            var actDescriptionChanged = GivenActDescriptionChanged(actTitle: "Modified Title");

            await WhenPublish(actDescriptionChanged);
            await WhenPublish(showAdded);

            await harness.Stop();

            repository.Shows.Single().ActDescription.Title.Should().Be("Modified Title");
        }

        [Fact]
        public async Task WhenVenueDescriptionIsChangedAfterShowIsAdded_ThenShowIsUpdated()
        {
            var showAdded = GivenShowAdded(venueName: "Original Name", venueDescriptionAge: 1);
            var venueDescriptionChanged = GivenVenueDescriptionChanged(venueName: "Modified Name");

            await WhenPublish(showAdded);
            await WhenPublish(venueDescriptionChanged);

            await harness.Stop();

            repository.Shows.Single().VenueDescription.Name.Should().Be("Modified Name");
        }

        [Fact]
        public async Task WhenVenueDescriptionChangeArrivesBeforeAfterShowAdded_ThenShowUsesLatestDescription()
        {
            var showAdded = GivenShowAdded(venueName: "Original Name", venueDescriptionAge: 1);
            var venueDescriptionChanged = GivenVenueDescriptionChanged(venueName: "Modified Name");

            await WhenPublish(venueDescriptionChanged);
            await WhenPublish(showAdded);

            await harness.Stop();

            repository.Shows.Single().VenueDescription.Name.Should().Be("Modified Name");
        }

        [Fact]
        public async Task WhenVenueLocationIsChangedAfterShowIsAdded_ThenShowIsUpdated()
        {
            var showAdded = GivenShowAdded(latitude: 0.0f, venueLocationAge: 1);
            var venueLocationChanged = GivenVenueLocationChanged(latitude: 45.0f);

            await WhenPublish(showAdded);
            await WhenPublish(venueLocationChanged);

            await harness.Stop();

            repository.Shows.Single().VenueLocation.Latitude.Should().Be(45.0f);
        }

        [Fact]
        public async Task WhenVenueLocationChangeArrivesBeforeAfterShowAdded_ThenShowUsesLatestLocation()
        {
            var showAdded = GivenShowAdded(latitude: 0.0f, venueLocationAge: 1);
            var venueLocationChanged = GivenVenueLocationChanged(latitude: 45.0f);

            await WhenPublish(venueLocationChanged);
            await WhenPublish(showAdded);

            await harness.Stop();

            repository.Shows.Single().VenueLocation.Latitude.Should().Be(45.0f);
        }

        private ShowAdded GivenShowAdded(
            string actTitle = "New Act",
            int actDescriptionAge = 0,
            string venueName = "New Venue",
            int venueDescriptionAge = 0,
            float latitude = 0.0f,
            int venueLocationAge = 0)
        {
            return new ShowAdded
            {
                act = new ActRepresentation
                {
                    actGuid = actGuid,
                    description = GivenActDescription(actTitle, actDescriptionAge)
                },
                venue = new VenueRepresentation
                {
                    venueGuid = venueGuid,
                    description = GivenVenueDescription(venueName, venueDescriptionAge),
                    location = GivenVenueLocation(latitude, venueLocationAge),
                    timeZone = new VenueTimeZoneRepresentation
                    {
                        timeZone = "UTC",
                        modifiedDate = DateTime.UtcNow
                    }
                },
                show = new ShowRepresentation
                {
                    startTime = DateTimeOffset.Now
                }
            };
        }

        private VenueDescriptionChanged GivenVenueDescriptionChanged(
            string venueName = "New Venue",
            int venueDescriptionAge = 0)
        {
            return new VenueDescriptionChanged
            {
                venueGuid = venueGuid,
                description = GivenVenueDescription(venueName, venueDescriptionAge)
            };
        }

        private VenueLocationChanged GivenVenueLocationChanged(
            float latitude = 0.0f,
            int venueLocationAge = 0)
        {
            return new VenueLocationChanged
            {
                venueGuid = venueGuid,
                location = GivenVenueLocation(latitude, venueLocationAge)
            };
        }

        private ActDescriptionChanged GivenActDescriptionChanged(
            string actTitle = "New Act",
            int actDescriptionAge = 0)
        {
            return new ActDescriptionChanged
            {
                actGuid = actGuid,
                description = GivenActDescription(actTitle, actDescriptionAge)
            };
        }

        private static VenueDescriptionRepresentation GivenVenueDescription(
            string venueName,
            int venueDescriptionAge)
        {
            return new VenueDescriptionRepresentation
            {
                name = venueName,
                city = "Anytown, VA",
                modifiedDate = DateTime.UtcNow.AddDays(-venueDescriptionAge)
            };
        }

        private static VenueLocationRepresentation GivenVenueLocation(
            float latitude,
            int locationAge)
        {
            return new VenueLocationRepresentation
            {
                latitude = latitude,
                longitude = -45,
                modifiedDate = DateTime.UtcNow.AddDays(-locationAge)
            };
        }

        private static ActDescriptionRepresentation GivenActDescription(
            string actTitle,
            int actDescriptionAge)
        {
            return new ActDescriptionRepresentation
            {
                title = actTitle,
                imageHash = "abc123",
                modifiedDate = DateTime.UtcNow.AddDays(-actDescriptionAge)
            };
        }

        private async Task WhenPublish(object message)
        {
            await harness.Bus.Publish(message);
        }
    }
}
