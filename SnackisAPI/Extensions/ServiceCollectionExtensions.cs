using SnackisAPI.Repositories;
using SnackisAPI.Services;

namespace SnackisAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPrivateMessageService, PrivateMessageService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
