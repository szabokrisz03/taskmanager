using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TaskManager.Srv.Model.DataContext;
using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.UtilityServices;

public class UserService : IUserService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<ManagerContext> dbContextFactory;

    public UserService(
        IMapper mapper,
        IDbContextFactory<ManagerContext> dbContextFactory)
    {
        this.mapper = mapper;
        this.dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc cref="IUserService.CreateUser(UserViewModel)"/>
    public async Task CreateUser(UserViewModel userModel)
    {
        if (await ExistsUser(userModel.UserName))
        {
            return;
        }

        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var user = mapper.Map<User>(userModel);
            await dbcx.User.AddAsync(user);
            await dbcx.SaveChangesAsync();
            dbcx.Entry(user).State = EntityState.Detached;
        }
    }

    /// <inheritdoc cref="IUserService.GetUser(string)"/>
    public async Task<UserViewModel?> GetUser(string userName)
    {
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var user = await dbcx.User.AsNoTracking().Where(u => u.UserName == userName).SingleOrDefaultAsync();
            return mapper.Map<UserViewModel>(user);
        }
    }

    /// <inheritdoc cref="IUserService.ExistsUser(string)"/>
    public async Task<bool> ExistsUser(string userName)
    {
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            return await dbcx.User.AsNoTracking().Where(u => u.UserName == userName).AnyAsync();
        }
    }

    /// <inheritdoc cref="IUserService.EnsureUserExists(string)"/>
    public async Task EnsureUserExists(string userName)
    {
        if (await ExistsUser(userName))
        {
            return;
        }

        var userVm = new UserViewModel()
        {
            UserName = userName,
        };
        await CreateUser(userVm);
    }
}
