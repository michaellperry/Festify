using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Acts;

public class ActRemovedConfiguration : IEntityTypeConfiguration<ActRemoved>
{
    public void Configure(EntityTypeBuilder<ActRemoved> builder)
    {
        builder.HasAlternateKey(actRemoved => new { actRemoved.ActId, actRemoved.RemovedDate });
    }
}