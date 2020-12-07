using FluentAssertions;
using Festify.Promotion.Data;
using Festify.Promotion.Venues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Festify.Promotion.UnitTest
{
    public class VenueTests
    {
        [Fact]
        public async Task VenuesInitiallyEmpty()
        {
            List<VenueInfo> venues = await venueQueries.ListVenues();
            venues.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenAddVenue_VenueIsReturned()
        {
            var venueGuid = Guid.NewGuid();
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));

            var venues = await venueQueries.ListVenues();
            venues.Should().Contain(venue => venue.VenueGuid == venueGuid);
        }

        [Fact]
        public async Task WhenAddVenueTwice_OneVenueIsAdded()
        {
            var venueGuid = Guid.NewGuid();
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));

            var venues = await venueQueries.ListVenues();
            venues.Count.Should().Be(1);
        }

        [Fact]
        public async Task WhenSetVenueDescription_VenueDescriptionIsReturned()
        {
            var venueGuid = Guid.NewGuid();
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));

            var venue = await venueQueries.GetVenue(venueGuid);
            venue.Name.Should().Be("American Airlines Center");
        }

        [Fact]
        public async Task WhenSetVenueToSameDescription_NothingIsSaved()
        {
            var venueGuid = Guid.NewGuid();
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));
            var firstSnapshot = await venueQueries.GetVenue(venueGuid);

            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center",
                firstSnapshot.LastModifiedTicks));
            var secondSnapshot = await venueQueries.GetVenue(venueGuid);
            secondSnapshot.LastModifiedTicks.Should().Be(firstSnapshot.LastModifiedTicks);
        }

        [Fact]
        public async Task WhenVenueIsModifiedConcurrently_ExceptionIsThrown()
        {
            var venueGuid = Guid.NewGuid();
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));
            var venue = await venueQueries.GetVenue(venueGuid);

            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "Change 1", venue.LastModifiedTicks));
            Func<Task> concurrentSave = () =>
                venueCommands.SaveVenue(VenueInfoWith(venueGuid, "Change 2", venue.LastModifiedTicks));

            await concurrentSave.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task WhenDeleteVenue_VenueIsNotReturned()
        {
            var venueGuid = Guid.NewGuid();
            await venueCommands.SaveVenue(VenueInfoWith(venueGuid, "American Airlines Center"));

            await venueCommands.DeleteVenue(venueGuid);

            var venue = await venueQueries.GetVenue(venueGuid);
            venue.Should().BeNull();
            var venues = await venueQueries.ListVenues();
            venues.Should().BeEmpty();
        }

        private static VenueInfo VenueInfoWith(Guid venueGuid, string name, long lastModifiedTicks = 0)
        {
            return new VenueInfo
            {
                VenueGuid = venueGuid,
                Name = name,
                City = "Dallas, TX",
                LastModifiedTicks = lastModifiedTicks
            };
        }

        private VenueQueries venueQueries;
        private VenueCommands venueCommands;

        public VenueTests()
        {
            var repository = new PromotionContext(new DbContextOptionsBuilder<PromotionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options, null);

            venueQueries = new VenueQueries(repository);
            venueCommands = new VenueCommands(repository);
        }
    }
}