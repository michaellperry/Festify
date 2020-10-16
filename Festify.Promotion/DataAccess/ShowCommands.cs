using Festify.Promotion.DataAccess.Entities;
using System;
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
            await repository.AddAsync(new Show
            {
                ShowGuid = showGuid
            });
            await repository.SaveChangesAsync();
        }
    }
}