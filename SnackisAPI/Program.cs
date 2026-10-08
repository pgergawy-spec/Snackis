
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Exceptions;
using SnackisAPI.Models;
namespace SnackisAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<SnackisDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<SnackisDbContext>();


        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddOpenApi();

        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
