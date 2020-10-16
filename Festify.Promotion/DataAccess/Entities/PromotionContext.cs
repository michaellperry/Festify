using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.DataAccess.Entities
{
    public class PromotionContext : DbContext
    {
        public PromotionContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
    }
}