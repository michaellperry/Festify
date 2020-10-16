using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Festify.Promotion.Projections;
using Festify.Promotion.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;
using Festify.Promotion.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.Test
{
    public class ShowTests
    {
        [Fact]
        public async Task ShowsInitiallyEmpty()
        {
            List<ShowProjection> shows = await showQueries.GetAllShows();
            shows.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenAddShow_ShowIsReturned()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);

            var shows = await showQueries.GetAllShows();
            shows.Should().Contain(show => show.ShowGuid == showGuid);
        }

        private ShowQueries showQueries;
        private ShowCommands showCommands;

        public ShowTests()
        {
            var repository = new PromotionContext(new DbContextOptionsBuilder()
                .UseInMemoryDatabase(nameof(ShowTests))
                .Options);

            showQueries = new ShowQueries(repository);
            showCommands = new ShowCommands(repository);
        }
    }
}
