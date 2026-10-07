using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnackisAPI.Models;

namespace SnackisAPI.Data.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(topic => topic.Name).HasMaxLength(100);
        builder.Property(topic => topic.Slug).HasMaxLength(100);
        builder.HasIndex(topic => topic.Slug).IsUnique();

        builder.HasOne(topic => topic.Parent).WithMany()
            .HasForeignKey(topic => topic.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}