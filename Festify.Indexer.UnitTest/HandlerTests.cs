using FluentAssertions;
using Festify.Promotion.Messages.Acts;
using Festify.Promotion.Messages.Shows;
using Festify.Promotion.Messages.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Festify.Indexer.UnitTest
{
    public class HandlerTests
    {
        private readonly InMemoryRepository repository;
        private readonly ShowAddedHandler showAddedHandler;
        private readonly ActDescriptionChangedHandler actDescriptionChangedHandler;

        private readonly Guid actGuid = Guid.NewGuid();
        private readonly Guid venueGuid = Guid.NewGuid();

        public HandlerTests()
        {
            repository = new InMemoryRepository();
            showAddedHandler = new ShowAddedHandler(repository);
            actDescriptionChangedHandler = new ActDescriptionChangedHandler(repository);
        }

        [Fact]
        public async Task WhenShowIsAdded_ShowIsInIndex()
        {
            var showAdded = GivenShowAdded();
            await showAddedHandler.Handle(showAdded);

            repository.Shows.Should().BeEquivalentTo(new List<ShowAdded> { showAdded });
        }

        [Fact]
        public async Task WhenActDescriptionIsChangedAfterShowIsAdded_ThenShowIsUpdated()
        {
            var showAdded = GivenShowAdded(actTitle: "Original Title", actDescriptionAge: 1);
            await showAddedHandler.Handle(showAdded);
            var actDescriptionChanged = GivenActDescriptionChanged(actTitle: "Modified Title");
            await actDescriptionChangedHandler.Handle(actDescriptionChanged);

            repository.Shows.Single().act.description.title.Should().Be("Modified Title");
        }

        private ShowAdded GivenShowAdded(
            string actTitle = "New Act",
            int actDescriptionAge = 0)
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
                    description = new VenueDescriptionRepresentation
                    {
                        name = "New Venue",
                        city = "Anytown, VA",
                        modifiedDate = DateTime.UtcNow
                    },
                    location = new VenueLocationRepresentation
                    {
                        latitude = 123,
                        longitude = -45,
                        modifiedDate = DateTime.UtcNow
                    },
                    timeZone = new VenueTimeZoneRepresentation
                    {
                        timeZone = "UTC",
                        modifiedDate = DateTime.UtcNow
                    }
                }
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

        private static ActDescriptionRepresentation GivenActDescription(
            string actTitle = "New Act",
            int actDescriptionAge = 0)
        {
            return new ActDescriptionRepresentation
            {
                title = actTitle,
                imageHash = "abc123",
                modifiedDate = DateTime.UtcNow.AddDays(-actDescriptionAge)
            };
        }
    }
}
