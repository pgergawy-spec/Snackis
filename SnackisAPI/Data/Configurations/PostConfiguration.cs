using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnackisAPI.Models;

namespace SnackisAPI.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(post => post.Title).HasMaxLength(150);
        builder.Property(post => post.Content).HasMaxLength(4000);

        builder.HasOne(post => post.User).WithMany()
            .HasForeignKey(post => post.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(post => post.Topic).WithMany()
            .HasForeignKey(post => post.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}