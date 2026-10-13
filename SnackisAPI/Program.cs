
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Exceptions;
using SnackisAPI.Models;
using SnackisAPI.Startup;
namespace SnackisAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<SnackisDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddIdentityCore<User>(options => options.User.RequireUniqueEmail = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<SnackisDbContext>();


        ServiceRegistration.AddRepositories(builder.Services);
        ServiceRegistration.AddServices(builder.Services);
        ServiceRegistration.AddSeeding(builder.Services, builder.Configuration);
        ServiceRegistration.AddJwtAuthentication(builder.Services, builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        await DatabaseStartup.SeedDatabaseAsync(app);


        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context => context.Context.Response.Headers.XContentTypeOptions = "nosniff"
        });
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        await app.RunAsync();
    }
}
