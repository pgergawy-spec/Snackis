using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnackisAPI.Models;

namespace SnackisAPI.Data.Configurations;

public class PrivateMessageConfiguration : IEntityTypeConfiguration<PrivateMessage>
{
    public void Configure(EntityTypeBuilder<PrivateMessage> builder)
    {
        builder.Property(message => message.Content).HasMaxLength(2000);

        builder.HasOne(message => message.Sender).WithMany()
            .HasForeignKey(message => message.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(message => message.Receiver).WithMany()
            .HasForeignKey(message => message.ReceiverUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}