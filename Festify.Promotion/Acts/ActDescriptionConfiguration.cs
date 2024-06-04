using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Acts;

public class ActDescriptionConfiguration : IEntityTypeConfiguration<ActDescription>
{
    public void Configure(EntityTypeBuilder<ActDescription> builder)
    {
        builder.HasAlternateKey(actDescription => new { actDescription.ActId, actDescription.ModifiedDate });
    }
}