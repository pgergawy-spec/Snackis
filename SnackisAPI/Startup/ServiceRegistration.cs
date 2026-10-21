using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SnackisAPI.Repositories;
using SnackisAPI.Seeding;
using SnackisAPI.Services;
using SnackisAPI.Settings;
using System.Text;

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
    // services = DI-containern där JWT-autentiseringen registreras.
    // configuration = inställningarna (appsettings, user secrets, Azure).
    // Metoden behöver JWT-nyckeln för att kunna kontrollera att en token är äkta (signaturen).
    // så metoden kan läsa JWT-nyckeln och reglerna vid start.
    {
        var jwtSection = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtSection);

        var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();
        if (jwtSettings.Key.Length < MinimumJwtKeyLength)
            throw new InvalidOperationException($"Jwt:Key is missing or shorter than {MinimumJwtKeyLength} characters.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,// Kontrollera vem som skapade token
                ValidIssuer = jwtSettings.Issuer,// Måste vara vårt API
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),// Gör nyckeltexten till bytes och packar den som en symmetrisk nyckel (samma nyckel signerar och kontrollerar)
                ClockSkew = TimeSpan.FromSeconds(30)// Marginal för små klockskillnader. Utan raden gäller 5 min, valde 30 sek.
            });

        services.AddAuthorization();
    }
}
