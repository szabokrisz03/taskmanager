using Microsoft.Extensions.Caching.Memory;

using System.Collections.Immutable;

using TaskManager.Srv.Extensions;
using TaskManager.Srv.Model.DTO;
using TaskManager.Srv.Utilities;

namespace TaskManager.Srv.Services.AzdoServices;

public class AzdoTeamProjectService : IAzdoTeamProjectService
{
    private readonly IMemoryCache memoryCache;
    private readonly IHttpClientFactory httpClientFactory;

    public AzdoTeamProjectService(
        IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory)
    {
        this.memoryCache = memoryCache;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<ImmutableList<AzdoProjectDto>> GetTeamProjects()
    {
        var lst = await
        memoryCache.GetOrCreateAsync(CacheKeys.TEAM_PROJECTS, async (entry) =>
        {
            entry.SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(1));
            return await GetTeamProjectsFromAzdo()!;
        });

        return lst!;
    }

    private HttpRequestData MakeGetTeamProjectsRequest() =>
        new HttpRequestBuilder("projects")
        .SetQueryParam("stateFilter", "wellFormed")
        .SetQueryParam("api-version", "6.0")
        .Build();

    private async Task<ImmutableList<AzdoProjectDto>> GetTeamProjectsFromAzdo()
    {
        var request = MakeGetTeamProjectsRequest();
        using (var client = httpClientFactory.CreateClient(HttpClients.AZDO_ORG_GET))
        {
            var payload = (await HttpRequestManager.GetAsync<AzdoListDto<AzdoProjectDto>>(client, request))!;
            return payload.Value.Where(tp => tp.Name.StartsWith("t_")).ToImmutableList();
        }
    }

    public async Task<ImmutableList<AzdoIterationDto>> GetIterations(string project, string? team = null)
    {
        team ??= project + " Team";
        var lst = await
        memoryCache.GetOrCreateAsync(string.Format(CacheKeys.P_ITERATIONS, project, team), async (entry) =>
        {
            entry.SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(1));
            return await GetIterationsFromAzdo(project, team);
        });

        return lst!;
    }

    private HttpRequestData MakeListIterationsRequest() =>
        new HttpRequestBuilder("work/teamsettings/iterations")
        .SetQueryParam("api-version", "6.0")
        .Build();

    private async Task<ImmutableList<AzdoIterationDto>> GetIterationsFromAzdo(string project, string team)
    {
        var request = MakeListIterationsRequest();
        using (var client = httpClientFactory.CreateClient(HttpClients.AZDO_ORG_GET))
        {
            var result = (await HttpRequestManager.GetAsync<AzdoListDto<AzdoIterationDto>>(client, request, $"{project}/{team}"))!;
            return result.Value.ToImmutableList();
        }
    }
}