using Festify.Promotion.Data;
using Festify.Promotion.Venues;

namespace Festify.Promotion.Shows;

public class ShowCommands
{
    private PromotionContext repository;
    private ActCommands actCommands;
    private VenueCommands venueCommands;

    public ShowCommands(PromotionContext repository, ActCommands actCommands, VenueCommands venueCommands)
    {
        this.repository = repository;
        this.actCommands = actCommands;
        this.venueCommands = venueCommands;
    }

    public async Task ScheduleShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
        await GetOrInsertShow(actGuid, venueGuid, startTime);
        await repository.SaveChangesAsync();
    }

    public async Task CancelShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
        var show = await GetOrInsertShow(actGuid, venueGuid, startTime);
        await repository.AddAsync(new ShowCancelled
        {
            Show = show,
            CancelledDate = DateTime.UtcNow
        });
        await repository.SaveChangesAsync();
    }

    public async Task<Show> GetOrInsertShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
        var show = repository.Set<Show>()
            .Where(show =>
                show.Act.ActGuid == actGuid &&
                show.Venue.VenueGuid == venueGuid &&
                show.StartTime == startTime)
            .SingleOrDefault();
        if (show == null)
        {
            show = new Show
            {
                Act = await actCommands.GetOrInsertAct(actGuid),
                Venue = await venueCommands.GetOrInsertVenue(venueGuid),
                StartTime = startTime
            };
            await repository.AddAsync(show);
        }

        return show;
    }
}