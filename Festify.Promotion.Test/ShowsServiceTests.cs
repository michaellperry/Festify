using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Festify.Promotion.Projections;
using Festify.Promotion.Services;

namespace Festify.Promotion.Test
{
    public class ShowsServiceTests
    {
        [Fact]
        public void ShowsInitiallyEmpty()
        {
            List<ShowProjection> shows = showsService.GetAllShows();
            shows.Should().BeEmpty();
        }

        private ShowsService showsService;

        public ShowsServiceTests()
        {
            showsService = new ShowsService();
        }
    }
}
