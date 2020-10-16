using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Festify.Promotion.Projections;
using Festify.Promotion.Services;
using System.Threading.Tasks;

namespace Festify.Promotion.Test
{
    public class ShowsServiceTests
    {
        [Fact]
        public async Task ShowsInitiallyEmpty()
        {
            List<ShowProjection> shows = await showsService.GetAllShows();
            shows.Should().BeEmpty();
        }

        private ShowsService showsService;

        public ShowsServiceTests()
        {
            showsService = new ShowsService();
        }
    }
}
