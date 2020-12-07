using Festify.Promotion.Data;
using Festify.Promotion.Venues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Festify.Promotion.Shows
{
    public class ShowQueries
    {
        private PromotionContext repository;

        public ShowQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<List<ShowInfo>> ListShows(Guid actGuid)
        {
            var result = await repository.Show
                .Where(show =>
                    show.Act.ActGuid == actGuid &&
                    !show.Cancelled.Any())
                .Select(show => new
                {
                    VenueGuid = show.Venue.VenueGuid,
                    VenueDescription = show.Venue.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault(),
                    StartTime = show.StartTime
                })
                .ToListAsync();

            return result.Select(show => new ShowInfo
            {
                ActGuid = actGuid,
                Venue = VenueInfo.FromEntities(show.VenueGuid, show.VenueDescription),
                StartTime = show.StartTime
            })
                .ToList();
        }

        public async Task<ShowInfo> GetShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
        {
            var result = await repository.Show
                .Where(show =>
                    show.Act.ActGuid == actGuid &&
                    show.Venue.VenueGuid == venueGuid &&
                    show.StartTime == startTime &&
                    !show.Cancelled.Any())
                .Select(show => new
                {
                    VenueGuid = show.Venue.VenueGuid,
                    VenueDescription = show.Venue.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .SingleOrDefaultAsync();

            return result == null ? null : new ShowInfo
            {
                ActGuid = actGuid,
                Venue = VenueInfo.FromEntities(result.VenueGuid, result.VenueDescription),
                StartTime = startTime
            };
        }
    }
}