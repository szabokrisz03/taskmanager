using Microsoft.EntityFrameworkCore;

using TaskManager.Srv.Model.DataContext;

namespace TaskManager.Srv.Extensions;

/// <summary>
/// Alkalmazás inicializálása
/// </summary>
public static class AppInitExtensions
{
    public static async Task MigrateDatabases(this IServiceProvider provider)
    {
        using (var serviceScope = provider.CreateScope())
        {
            var dbcx = await serviceScope!.ServiceProvider.GetService<IDbContextFactory<ManagerContext>>()!.CreateDbContextAsync();

            dbcx?.Database.Migrate();
        }
    }
}