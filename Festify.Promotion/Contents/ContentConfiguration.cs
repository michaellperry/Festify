using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festify.Promotion.Contents;

public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.HasKey(content => content.Hash);
        builder.Property(content => content.Binary).IsRequired();
    }
}