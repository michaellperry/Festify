using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.DataAccess.Entities
{
    public class PromotionContext : DbContext
    {
        public PromotionContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Show> Show { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Show>()
                .HasAlternateKey(show => new { show.ShowGuid });
        }
    }
}