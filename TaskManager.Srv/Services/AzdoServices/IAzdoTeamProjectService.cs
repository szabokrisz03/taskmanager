using System.Collections.Immutable;

using TaskManager.Srv.Model.DTO;

namespace TaskManager.Srv.Services.AzdoServices;

public interface IAzdoTeamProjectService
{
    Task<ImmutableList<AzdoProjectDto>> GetTeamProjects();
    Task<ImmutableList<AzdoIterationDto>> GetIterations(string project, string? team = null);
}