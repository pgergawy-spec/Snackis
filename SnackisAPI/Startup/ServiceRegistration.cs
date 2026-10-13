using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SnackisAPI.Repositories;
using SnackisAPI.Seeding;
using SnackisAPI.Services;
using SnackisAPI.Settings;

namespace SnackisAPI.Startup;

public static class ServiceRegistration
{
    private const int MinimumJwtKeyLength = 32;

    public static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
    }

    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPrivateMessageService, PrivateMessageService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProfilePictureService, ProfilePictureService>();
    }

    public static void AddSeeding(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SeedSettings>(configuration.GetSection(SeedSettings.SectionName));
        services.AddScoped<DatabaseSeeder>();
    }

    public static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtSection);

        var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();
        if (jwtSettings.Key.Length < MinimumJwtKeyLength)
            throw new InvalidOperationException($"Jwt:Key is missing or shorter than {MinimumJwtKeyLength} characters.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.FromSeconds(30)
            });

        services.AddAuthorization();
    }
}
