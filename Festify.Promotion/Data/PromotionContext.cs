using Festify.Promotion.Shows;
using Festify.Promotion.Venues;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Festify.Promotion.Data;

public class PromotionContext : DbContext
{
    private readonly Dispatcher dispatcher;

    public PromotionContext(DbContextOptions<PromotionContext> options, Dispatcher dispatcher)
        : base(options)
    {
        this.dispatcher = dispatcher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PromotionContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesAdded = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();

        int result = await base.SaveChangesAsync(cancellationToken);

        if (dispatcher != null)
        {
            await dispatcher.DispatchAll(entitiesAdded);
        }

        return result;
    }

    public async Task<Act> GetOrInsertAct(Guid actGuid)
    {
        var act = Set<Act>()
            .Include(act => act.Descriptions)
            .Where(act => act.ActGuid == actGuid)
            .SingleOrDefault();
        if (act == null)
        {
            act = new Act
            {
                ActGuid = actGuid
            };
            await AddAsync(act);
        }

        return act;
    }

    public async Task<Venue> GetOrInsertVenue(Guid venueGuid)
    {
        var venue = Set<Venue>()
            .Include(venue => venue.Descriptions)
            .Include(venue => venue.Locations)
            .Include(venue => venue.TimeZones)
            .Where(venue => venue.VenueGuid == venueGuid)
            .SingleOrDefault();
        if (venue == null)
        {
            venue = new Venue
            {
                VenueGuid = venueGuid
            };
            await AddAsync(venue);
        }

        return venue;
    }

    public async Task<Show> GetOrInsertShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
        var show = Set<Show>()
            .Where(show =>
                show.Act.ActGuid == actGuid &&
                show.Venue.VenueGuid == venueGuid &&
                show.StartTime == startTime)
            .SingleOrDefault();
        if (show == null)
        {
            show = new Show
            {
                Act = await GetOrInsertAct(actGuid),
                Venue = await GetOrInsertVenue(venueGuid),
                StartTime = startTime
            };
            await base.AddAsync(show);
        }

        return show;
    }
}