using Festify.Promotion.Data;
using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.Venues;

public class VenueCommands
{
    private readonly PromotionContext repository;

    public VenueCommands(PromotionContext repository)
    {
        this.repository = repository;
    }

    public async Task SaveVenue(VenueInfo venueInfo)
    {
        var venue = await GetOrInsertVenue(venueInfo.VenueGuid);
        await SaveVenueDescription(venueInfo, venue);
        await SaveVenueLocation(venueInfo, venue);
        await SaveVenueTimeZone(venueInfo, venue);
    }

    public async Task DeleteVenue(Guid venueGuid)
    {
        var venue = await GetOrInsertVenue(venueGuid);
        await repository.AddAsync(new VenueRemoved
        {
            Venue = venue,
            RemovedDate = DateTime.UtcNow
        });
        await repository.SaveChangesAsync();
    }

    private async Task SaveVenueDescription(VenueInfo venueInfo, Venue venue)
    {
        var lastVenueDescription = repository.Set<VenueDescription>()
            .Where(venueDescription => venueDescription.VenueId == venue.VenueId)
            .OrderByDescending(description => description.ModifiedDate)
            .FirstOrDefault();

        if (lastVenueDescription == null ||
            lastVenueDescription.Name != venueInfo.Name ||
            lastVenueDescription.City != venueInfo.City)
        {
            var modifiedTicks = lastVenueDescription?.ModifiedDate.Ticks ?? 0;
            if (modifiedTicks != venueInfo.LastModifiedTicks)
            {
                throw new DbUpdateConcurrencyException(
                    "A new update has occurred since you loaded the page. Please refresh and try again.");
            }

            await repository.AddAsync(new VenueDescription
            {
                ModifiedDate = DateTime.UtcNow,
                Venue = venue,
                Name = venueInfo.Name,
                City = venueInfo.City
            });
            await repository.SaveChangesAsync();
        }
    }

    private async Task SaveVenueLocation(VenueInfo venueInfo, Venue venue)
    {
        switch ((venueInfo.Latitude, venueInfo.Longitude))
        {
            case (float latitude, float longitude):
                var lastVenueLocation = repository.Set<VenueLocation>()
                    .Where(venueLocation => venueLocation.VenueId == venue.VenueId)
                    .OrderByDescending(description => description.ModifiedDate)
                    .FirstOrDefault();

                if (lastVenueLocation == null ||
                    lastVenueLocation.Latitude != latitude ||
                    lastVenueLocation.Longitude != longitude)
                {
                    var modifiedTicks = lastVenueLocation?.ModifiedDate.Ticks ?? 0;
                    if (modifiedTicks != venueInfo.LocationLastModifiedTicks)
                    {
                        throw new DbUpdateConcurrencyException(
                            "A new update has occurred since you loaded the page. Please refresh and try again.");
                    }

                    await repository.AddAsync(new VenueLocation
                    {
                        ModifiedDate = DateTime.UtcNow,
                        Venue = venue,
                        Latitude = latitude,
                        Longitude = longitude
                    });
                    await repository.SaveChangesAsync();
                }

                break;
        }
    }

    private async Task SaveVenueTimeZone(VenueInfo venueInfo, Venue venue)
    {
        if (!string.IsNullOrEmpty(venueInfo.TimeZone))
        {
            var lastVenueTimeZone = repository.Set<VenueTimeZone>()
                .Where(venueTimeZone => venueTimeZone.VenueId == venue.VenueId)
                .OrderByDescending(description => description.ModifiedDate)
                .FirstOrDefault();

            if (lastVenueTimeZone == null ||
                lastVenueTimeZone.TimeZone != venueInfo.TimeZone)
            {
                var modifiedTicks = lastVenueTimeZone?.ModifiedDate.Ticks ?? 0;
                if (modifiedTicks != venueInfo.TimeZoneLastModifiedTicks)
                {
                    throw new DbUpdateConcurrencyException(
                        "A new update has occurred since you loaded the page. Please refresh and try again.");
                }

                await repository.AddAsync(new VenueTimeZone
                {
                    ModifiedDate = DateTime.UtcNow,
                    Venue = venue,
                    TimeZone = venueInfo.TimeZone
                });
                await repository.SaveChangesAsync();
            }
        }
    }

    public async Task<Venue> GetOrInsertVenue(Guid venueGuid)
    {
        var venue = repository.Set<Venue>()
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
            await repository.AddAsync(venue);
        }

        return venue;
    }
}