using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Shows;

public class ShowCancelledConfiguration : IEntityTypeConfiguration<ShowCancelled>
{
    public void Configure(EntityTypeBuilder<ShowCancelled> builder)
    {
        builder.HasAlternateKey(showCancelled => new { showCancelled.ShowId, showCancelled.CancelledDate });
    }
}