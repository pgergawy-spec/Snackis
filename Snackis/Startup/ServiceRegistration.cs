using Snackis.Services;

namespace Snackis.Startup;

public static class ServiceRegistration
{
    public static void AddApiServices(IServiceCollection services)
    {
        services.AddScoped<ITopicApiService, TopicApiService>();
        services.AddScoped<IPostApiService, PostApiService>();
        services.AddScoped<ICommentApiService, CommentApiService>();
        services.AddScoped<IAccountApiService, AccountApiService>();
    }
}
