using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Festify.Promotion.Data;

public class PromotionContext : DbContext
{
    private readonly Dispatcher dispatcher;

    public PromotionContext(DbContextOptions<PromotionContext> options, Dispatcher dispatcher)
        : base(options)
    {
        this.dispatcher = dispatcher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PromotionContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesAdded = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();

        int result = await base.SaveChangesAsync(cancellationToken);

        if (dispatcher != null)
        {
            await dispatcher.DispatchAll(entitiesAdded);
        }

        return result;
    }
}