using Festify.Promotion.Data;

namespace Festify.Promotion.Shows;

public class ShowCommands
{
    private PromotionContext repository;

    public ShowCommands(PromotionContext repository)
    {
            this.repository = repository;
        }

    public async Task ScheduleShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
            await repository.GetOrInsertShow(actGuid, venueGuid, startTime);
            await repository.SaveChangesAsync();
        }

    public async Task CancelShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
            var show = await repository.GetOrInsertShow(actGuid, venueGuid, startTime);
            await repository.AddAsync(new ShowCancelled
            {
                Show = show,
                CancelledDate = DateTime.UtcNow
            });
            await repository.SaveChangesAsync();
        }
}