using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Acts;

public class ActConfiguration : IEntityTypeConfiguration<Act>
{
    public void Configure(EntityTypeBuilder<Act> builder)
    {
        builder.HasAlternateKey(act => new { act.ActGuid });
    }
}