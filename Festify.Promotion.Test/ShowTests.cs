using Festify.Promotion.DataAccess;
using Festify.Promotion.DataAccess.Entities;
using Festify.Promotion.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
        public async Task WhenAddShowTwice_OneShowIsAdded()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);
            await showCommands.AddShow(showGuid);

            var shows = await showQueries.ListShows();
            shows.Count.Should().Be(1);
        }

        [Fact]
        public async Task WhenSetShowDescription_ShowDescriptionIsReturned()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);
            await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Gabriel Iglesias"));

            var shows = await showQueries.ListShows();
            var show = shows.Where(s => s.ShowGuid == showGuid).Single();
            show.Description.Title.Should().Be("Gabriel Iglesias");
        }

        [Fact]
        public async Task WhenChangeShowDescription_ShowDescriptionIsModified()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);
            await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Gabriel Iglesias"));
            var versionOne = await showQueries.GetShow(showGuid);
            await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Jeff Dunham", versionOne.Description.LastModifiedTicks));

            var shows = await showQueries.ListShows();
            var show = shows.Where(s => s.ShowGuid == showGuid).Single();
            show.Description.Title.Should().Be("Jeff Dunham");
        }

        [Fact]
        public async Task WhenBasedOnOldVersion_ChangeIsRejected()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Gabriel Iglesias"));
            var versionOne = await showQueries.GetShow(showGuid);
            await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Jeff Dunham", versionOne.Description.LastModifiedTicks));
            
            Func<Task> update = async () =>
            {
                await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Jeff Foxworthy", versionOne.Description.LastModifiedTicks));
            };
            update.Should().Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task GivenShowDoesNotExist_WhenSetShowDescription_SHowIsCreated()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.SetShowDescription(showGuid, ShowDescriptionWith("Gabriel Iglesias"));

            var shows = await showQueries.ListShows();
            var show = shows.Where(s => s.ShowGuid == showGuid).Single();
            show.Description.Title.Should().Be("Gabriel Iglesias");
        }

        [Fact]
        public async Task WhenRemoveShow_ShowIsNotReturned()
        {
            var showGuid = Guid.NewGuid();
            await showCommands.AddShow(showGuid);
            await showCommands.RemoveShow(showGuid);

            var shows = await showQueries.ListShows();
            shows.Should().BeEmpty();
        }

        private static ShowDescriptionModel ShowDescriptionWith(string title, long lastModifiedTicks = 0)
        {
            var random = new Random();
            var imageHash = new byte[512 / 8];
            random.NextBytes(imageHash);

            ShowDescriptionModel showDescription = new ShowDescriptionModel
            {
                Title = title,
                Date = new DateTime(2021, 8, 29, 1, 0, 0, DateTimeKind.Utc),
                City = "Durant, OK",
                Venue = "Choctaw Grand Theater",
                ImageHash = Convert.ToBase64String(imageHash),
                LastModifiedTicks = lastModifiedTicks
            };
            return showDescription;
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
