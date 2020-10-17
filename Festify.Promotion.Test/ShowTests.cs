using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Festify.Promotion.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Festify.Promotion.DataAccess;
using Festify.Promotion.DataAccess.Entities;
using System.Reflection;
using System.Linq;

namespace Festify.Promotion.Test
{
    public class ShowTests
    {
        [Fact]
        public async Task ShowsInitiallyEmpty()
        {
            List<ShowModel> shows = await showQueries.ListShows();
            shows.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenAddShow_ShowIsReturned()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);

            var shows = await showQueries.ListShows();
            shows.Should().Contain(show => show.ShowGuid == showGuid);
        }

        [Fact]
        public async Task WhenSetShowDescription_ShowDescriptionIsReturned()
        {
            var random = new Random();
            var imageHash = new byte[512 / 8];
            random.NextBytes(imageHash);

            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);
            await showCommands.SetShowDescription(showGuid, new ShowDescriptionModel
            {
                Title = "Gabriel Iglesias",
                Date = new DateTime(2021, 8, 29, 1, 0, 0, DateTimeKind.Utc),
                City = "Durant, OK",
                Venue = "Choctaw Grand Theater",
                ImageHash = Convert.ToBase64String(imageHash)
            });

            var shows = await showQueries.ListShows();
            var show = shows.Where(s => s.ShowGuid == showGuid).Single();
            show.Description.Title.Should().Be("Gabriel Iglesias");
        }

        private ShowQueries showQueries;
        private ShowCommands showCommands;

        public ShowTests()
        {
            var repository = new PromotionContext(new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

            showQueries = new ShowQueries(repository);
            showCommands = new ShowCommands(repository);
        }
    }
}
