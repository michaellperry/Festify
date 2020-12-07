using Festify.Promotion.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Festify.Promotion.Contents
{
    public class ContentQueries
    {
        private readonly PromotionContext repository;

        public ContentQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<Content> GetContent(string hash)
        {
            return await repository.Content
                .Where(c => c.Hash == hash)
                .SingleOrDefaultAsync();
        }
    }
}
