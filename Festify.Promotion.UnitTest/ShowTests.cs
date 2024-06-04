using FluentAssertions;
using Festify.Promotion.Acts;
using Festify.Promotion.Data;
using Festify.Promotion.Shows;
using Festify.Promotion.Venues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Festify.Promotion.UnitTest;

public class ShowTests
{
    [Fact]
    public async Task ActInitiallyHasNoShows()
    {
        var actGuid = await GivenAct();

        List<ShowInfo> shows = await showQueries.ListShows(actGuid);
        shows.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenShowIsScheduled_ShowIsReturned()
    {
        var actGuid = await GivenAct();
        var venueGuid = await GivenVenue();

        DateTimeOffset startTime = new DateTimeOffset(2021, 03, 21, 08, 00, 00, LocalOffset);
        await showCommands.ScheduleShow(actGuid, venueGuid, startTime);

        var shows = await showQueries.ListShows(actGuid);
        shows.Should().Contain(show => show.StartTime == startTime);
    }

    [Fact]
    public async Task WhenShowIsScheduledTwice_OneShowIsReturned()
    {
        var actGuid = await GivenAct();
        var venueGuid = await GivenVenue();

        DateTimeOffset startTime = new DateTimeOffset(2021, 03, 21, 08, 00, 00, LocalOffset);
        await showCommands.ScheduleShow(actGuid, venueGuid, startTime);
        await showCommands.ScheduleShow(actGuid, venueGuid, startTime);

        var shows = await showQueries.ListShows(actGuid);
        shows.Count.Should().Be(1);
    }

    [Fact]
    public async Task WhenShowIsCanceled_ShowIsNotReturned()
    {
        var actGuid = await GivenAct();
        var venueGuid = await GivenVenue();

        DateTimeOffset startTime = new DateTimeOffset(2021, 03, 21, 08, 00, 00, LocalOffset);
        await showCommands.ScheduleShow(actGuid, venueGuid, startTime);

        await showCommands.CancelShow(actGuid, venueGuid, startTime);

        var shows = await showQueries.ListShows(actGuid);
        shows.Should().BeEmpty();
    }

    private async Task<Guid> GivenAct()
    {
        var actGuid = Guid.NewGuid();
        var actModel = new ActInfo
        {
            ActGuid = actGuid,
            Title = "Test Act"
        };
        await actCommands.SaveAct(actModel);
        return actGuid;
    }

    private async Task<Guid> GivenVenue()
    {
        var venueGuid = Guid.NewGuid();
        var venueModel = new VenueInfo
        {
            VenueGuid = venueGuid,
            City = "Test City",
            Name = "Test Venue"
        };
        await venueCommands.SaveVenue(venueModel);
        return venueGuid;
    }

    static TimeSpan LocalOffset = TimeZoneInfo.Local.BaseUtcOffset;

    private ActCommands actCommands;
    private VenueCommands venueCommands;
    private ShowQueries showQueries;
    private ShowCommands showCommands;

    public ShowTests()
    {
        var repository = new PromotionContext(new DbContextOptionsBuilder<PromotionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options, null);

        actCommands = new ActCommands(repository);
        venueCommands = new VenueCommands(repository);
        showQueries = new ShowQueries(repository);
        showCommands = new ShowCommands(repository, actCommands, venueCommands);
    }
}