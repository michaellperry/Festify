using Festify.Promotion.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Festify.Promotion.Acts;

public class ActQueries
{
    private readonly PromotionContext repository;

    public ActQueries(PromotionContext repository)
    {
            this.repository = repository;
        }

    public async Task<List<ActInfo>> ListActs()
    {
            var result = await repository.Act
                .Where(act => !act.Removed.Any())
                .Select(act => new
                {
                    act.ActGuid,
                    Description = act.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return result
                .Select(row => MapActModel(row.ActGuid, row.Description))
                .ToList();
        }

    public async Task<ActInfo> GetAct(Guid actGuid)
    {
            var result = await repository.Act
                .Where(act => act.ActGuid == actGuid)
                .Select(act => new
                {
                    act.ActGuid,
                    Description = act.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .SingleOrDefaultAsync();

            return result == null ? null : MapActModel(result.ActGuid, result.Description);
        }

    private static ActInfo MapActModel(Guid actGuid, ActDescription actDescription)
    {
            return new ActInfo
            {
                ActGuid = actGuid,
                Title = actDescription?.Title,
                ImageHash = actDescription?.ImageHash,
                LastModifiedTicks = actDescription?.ModifiedDate.Ticks ?? 0
            };
        }
}