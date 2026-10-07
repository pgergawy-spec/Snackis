using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnackisAPI.Models;

namespace SnackisAPI.Data;

public class SnackisDbContext : IdentityDbContext<User>
{
    public SnackisDbContext(DbContextOptions<SnackisDbContext> options) : base(options)
    {

    }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PrivateMessage> PrivateMessages { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Topic> Topics { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(SnackisDbContext).Assembly);
    }

}
