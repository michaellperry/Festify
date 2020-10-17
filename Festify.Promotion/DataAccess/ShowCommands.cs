using Festify.Promotion.DataAccess.Entities;
using Festify.Promotion.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task SetShowDescription(Guid showGuid, ShowDescriptionModel showDescriptionModel)
        {
            var show = await GetOrInsertShow(showGuid);
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

        private async Task<Show> GetOrInsertShow(Guid showGuid)
        {
            var show = repository.Show
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