using Festify.Promotion.Contents;
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

    public DbSet<Act> Act { get; set; }
    public DbSet<ActDescription> ActDescription { get; set; }
    public DbSet<Venue> Venue { get; set; }
    public DbSet<VenueDescription> VenueDescription { get; set; }
    public DbSet<VenueLocation> VenueLocation { get; set; }
    public DbSet<VenueTimeZone> VenueTimeZone { get; set; }
    public DbSet<Show> Show { get; set; }
    public DbSet<Content> Content { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Act>()
            .HasAlternateKey(act => new { act.ActGuid });

        modelBuilder.Entity<ActRemoved>()
            .HasAlternateKey(actRemoved => new { actRemoved.ActId, actRemoved.RemovedDate });

        modelBuilder.Entity<ActDescription>()
            .HasAlternateKey(actDescription => new { actDescription.ActId, actDescription.ModifiedDate });

        modelBuilder.Entity<Venue>()
            .HasAlternateKey(venue => new { venue.VenueGuid });

        modelBuilder.Entity<VenueDescription>()
            .HasAlternateKey(venueDescription => new { venueDescription.VenueId, venueDescription.ModifiedDate });

        modelBuilder.Entity<VenueLocation>()
            .HasAlternateKey(venueLocation => new { venueLocation.VenueId, venueLocation.ModifiedDate });

        modelBuilder.Entity<VenueTimeZone>()
            .HasAlternateKey(venueTimeZone => new { venueTimeZone.VenueId, venueTimeZone.ModifiedDate });

        modelBuilder.Entity<Show>()
            .HasAlternateKey(show => new { show.ActId, show.VenueId, show.StartTime });

        modelBuilder.Entity<ShowCancelled>()
            .HasAlternateKey(showCancelled => new { showCancelled.ShowId, showCancelled.CancelledDate });

        modelBuilder.Entity<Content>()
            .HasKey(content => content.Hash);
        modelBuilder.Entity<Content>()
            .Property(content => content.Binary)
            .IsRequired();
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
        var act = Act
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
        var venue = Venue
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
        var show = Show
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