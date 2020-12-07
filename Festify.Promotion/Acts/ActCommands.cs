using Festify.Promotion.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Festify.Promotion.Acts
{
    public class ActCommands
    {
        private readonly PromotionContext repository;

        public ActCommands(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task SaveAct(ActInfo actModel)
        {
            var act = await repository.GetOrInsertAct(actModel.ActGuid);
            var lastActDescription = act.Descriptions
                .OrderByDescending(description => description.ModifiedDate)
                .FirstOrDefault();

            if (lastActDescription == null ||
                lastActDescription.Title != actModel.Title ||
                lastActDescription.ImageHash != actModel.ImageHash)
            {
                var modifiedTicks = lastActDescription?.ModifiedDate.Ticks ?? 0;
                if (modifiedTicks != actModel.LastModifiedTicks)
                {
                    throw new DbUpdateConcurrencyException("A new update has occurred since you loaded the page. Please refresh and try again.");
                }

                await repository.AddAsync(new ActDescription
                {
                    ModifiedDate = DateTime.UtcNow,
                    Act = act,
                    Title = actModel.Title,
                    ImageHash = actModel.ImageHash
                });
                await repository.SaveChangesAsync();
            }
        }

        public async Task RemoveAct(Guid actGuid)
        {
            var act = await repository.GetOrInsertAct(actGuid);
            await repository.AddAsync(new ActRemoved
            {
                Act = act,
                RemovedDate = DateTime.UtcNow
            });
            await repository.SaveChangesAsync();
        }
    }
}