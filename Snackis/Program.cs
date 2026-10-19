using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Snackis.Filters;
using Snackis.Services;
using Snackis.Settings;
using Snackis.Startup;

namespace Snackis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        var apiBaseUrl = builder.Configuration["Api:BaseUrl"]
            ?? throw new Exception("Api:BaseUrl saknas i konfigurationen");

        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SectionName));

        builder.Services.AddHttpClient(ApiServiceBase.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        ServiceRegistration.AddApiServices(builder.Services);
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Home/Error";
                options.SlidingExpiration = false;
            }
            );

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
