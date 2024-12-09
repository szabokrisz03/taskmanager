using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

using TaskManager.Srv.Model.DataContext;
using TaskManager.Srv.Model.Options;
using TaskManager.Srv.Services.AzdoServices;
using TaskManager.Srv.Services.DiscussionServices;
using TaskManager.Srv.Services.MilestoneServices;
using TaskManager.Srv.Services.ProjectServices;
using TaskManager.Srv.Services.TaskServices;
using TaskManager.Srv.Services.UtilityServices;
using TaskManager.Srv.Services.WiLinkService;
using TaskManager.Srv.Services.WiServices;

namespace TaskManager.Srv.Extensions;

/// <summary>
/// Regisztrálja a szolgáltatásokat
/// </summary>
public static class ServiceRegistrationExtension
{
    public static IServiceCollection RegisterDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConns = configuration.GetConnectionString("default");
        services.AddPooledDbContextFactory<ManagerContext>(options => options
            .UseSqlServer(defaultConns)
            .UseSnakeCaseNamingConvention());

        return services;
    }

    public static IServiceCollection RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = new AppSettings();
        configuration.Bind(appSettings);

        // org base, data-fetch (srv usr. winauth)
        services.AddHttpClient(HttpClients.AZDO_ORG_GET, httpClient =>
        {
            httpClient.BaseAddress = new Uri(appSettings.Remotes.AzdoOrg + "_apis/");

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptEncoding, "utf-8");
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            UseDefaultCredentials = true
        });

        return services;
    }

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddTransient<IClaimsTransformation, ClaimsTransformation>();
        services.AddTransient<IAzdoTeamProjectService, AzdoTeamProjectService>();
        services.AddScoped<IProjectAdminService, ProjectAdminService>();
        services.AddScoped<IProjectDisplayService, ProjectDisplayService>();
        services.AddScoped<IProjectViewService, ProjectViewService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWiLinkTemplateService, WiLinkTemplateService>();
        services.AddScoped<IWiLinkTemplateViewService, WiLinkTemplateViewService>();
        services.AddScoped<ITaskDisplayService, TaskDisplayService>();
        services.AddScoped<ITaskViewService, TaskViewService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IMilestoneService, MilestoneService>();
        services.AddScoped<IMilestoneDisplayService, MilestoneDisplayService>();
        services.AddScoped<IMilestoneViewService, MilestoneViewService>();
        services.AddScoped<IWiStateService, WiStateService>();
        services.AddScoped<IWiService, WiService>();
        services.AddScoped<IAzdoUserService, AzdoUserService>();

        return services;
    }
}