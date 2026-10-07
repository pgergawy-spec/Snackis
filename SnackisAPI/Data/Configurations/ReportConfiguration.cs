using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnackisAPI.Models;

namespace SnackisAPI.Data.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(report => report.Reason).HasMaxLength(500);

        builder.HasOne(report => report.User).WithMany()
            .HasForeignKey(report => report.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(report => report.Post).WithMany()
            .HasForeignKey(report => report.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}