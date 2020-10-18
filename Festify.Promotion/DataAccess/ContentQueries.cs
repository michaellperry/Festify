using Festify.Promotion.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Festify.Promotion.DataAccess
{
    public class ContentQueries
    {
        private readonly PromotionContext repository;

        public ContentQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<byte[]> GetContent(string hash)
        {
            var content = await repository.Content
                .Where(c => c.Hash == hash)
                .SingleOrDefaultAsync();

            return content?.Binary;
        }
    }
}
