using Festify.Promotion.Services.Entities;
using System;
using System.Threading.Tasks;

namespace Festify.Promotion.Services
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
            await repository.AddAsync(new Show
            {
                ShowGuid = showGuid
            });
            await repository.SaveChangesAsync();
        }
    }
}