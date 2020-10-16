using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Festify.Promotion.Projections;
using Festify.Promotion.Services;
using System.Threading.Tasks;

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

        private ShowQueries showQueries;

        public ShowTests()
        {
            showQueries = new ShowQueries();
        }
    }
}
