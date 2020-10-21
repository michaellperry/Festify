using Festify.Promotion.DataAccess.Entities;
using Festify.Promotion.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.DataAccess
{
    public class ShowCommands
    {
        private readonly PromotionContext repository;

        public ShowCommands(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task AddShow(Guid showGuid)
        {
            await GetOrInsertShow(showGuid);
            await repository.SaveChangesAsync();
        }

        public async Task RemoveShow(Guid showGuid)
        {
            var show = await GetOrInsertShow(showGuid);
            await repository.AddAsync(new ShowRemoved
            {
                Show = show,
                RemovedDate = DateTime.UtcNow
            });
            await repository.SaveChangesAsync();
        }

        public async Task SetShowDescription(Guid showGuid, ShowDescriptionModel showDescriptionModel)
        {
            var show = await GetOrInsertShow(showGuid);
            var lastShowDescription = show.Descriptions
                .OrderByDescending(description => description.ModifiedDate)
                .FirstOrDefault();
            var modifiedTicks = ToTicks(lastShowDescription?.ModifiedDate);
            if (modifiedTicks != showDescriptionModel.LastModifiedTicks)
            {
                throw new DbUpdateConcurrencyException("A new update has occurred since you loaded the page. Please refresh and try again.");
            }

            await repository.AddAsync(new ShowDescription
            {
                ModifiedDate = DateTime.UtcNow,
                Show = show,
                Title = showDescriptionModel.Title,
                Date = showDescriptionModel.Date,
                City = showDescriptionModel.City,
                Venue = showDescriptionModel.Venue,
                ImageHash = showDescriptionModel.ImageHash
            });
            await repository.SaveChangesAsync();
        }

        private long ToTicks(DateTime? optionalDate)
        {
            switch (optionalDate)
            {
                case DateTime date:
                    return date.Ticks;
                case null:
                    return 0;
            }
        }

        private async Task<Show> GetOrInsertShow(Guid showGuid)
        {
            var show = repository.Show
                .Include(show => show.Descriptions)
                .Where(show => show.ShowGuid == showGuid)
                .SingleOrDefault();
            if (show == null)
            {
                show = new Show
                {
                    ShowGuid = showGuid
                };
                await repository.AddAsync(show);
            }

            return show;
        }
    }
}