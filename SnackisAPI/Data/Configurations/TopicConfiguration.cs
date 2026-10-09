using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnackisAPI.Helpers;
using SnackisAPI.Models;

namespace SnackisAPI.Data.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(topic => topic.Name).HasMaxLength(FieldLengths.TopicName);
        builder.Property(topic => topic.UrlName).HasMaxLength(FieldLengths.TopicName);
        builder.HasIndex(topic => topic.UrlName).IsUnique();

        builder.HasOne(topic => topic.Parent).WithMany()
            .HasForeignKey(topic => topic.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}