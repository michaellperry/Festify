using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festify.Promotion.Data;
using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.Venues
{
    public class VenueQueries
    {
        private readonly PromotionContext repository;

        public VenueQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<List<VenueInfo>> ListVenues()
        {
            var result = await repository.Venue
                .Where(venue => !venue.Removed.Any())
                .Select(venue => new
                {
                    venue.VenueGuid,
                    Description = venue.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return result
                .Select(row => VenueInfo.FromEntities(row.VenueGuid, row.Description))
                .ToList();
        }

        public async Task<VenueInfo> GetVenue(Guid venueGuid)
        {
            var result = await repository.Venue
                .Where(venue => venue.VenueGuid == venueGuid &&
                    !venue.Removed.Any())
                .Select(venue => new
                {
                    venue.VenueGuid,
                    Description = venue.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault(),
                    Location = venue.Locations
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault(),
                    TimeZone = venue.TimeZones
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .SingleOrDefaultAsync();

            return result == null ? null : VenueInfo.FromEntities(
                result.VenueGuid, result.Description, result.Location, result.TimeZone);
        }

        public static VenueInfo MapVenue(Guid venueGuid, VenueDescription venueDescription)
        {
            return new VenueInfo
            {
                VenueGuid = venueGuid,
                Name = venueDescription?.Name,
                City = venueDescription?.City,
                LastModifiedTicks = venueDescription?.ModifiedDate.Ticks ?? 0
            };
        }
    }
}