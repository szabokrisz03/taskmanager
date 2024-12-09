using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.UtilityServices;

/// <summary>
/// Felhasználó létrehozására.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Felhasználó lekérdezése
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    /// <returns>A megtalált felhasználó</returns>
    Task<UserViewModel?> GetUser(string userName);

    /// <summary>
    /// Felhasználó létrehozása.
    /// </summary>
    /// <param name="userModel">A létrehozandó felhasználó</param>
    Task CreateUser(UserViewModel userModel);

    /// <summary>
    /// Megnézi, hogy létezik-e az adott felhasználó.
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    /// <returns>True, ha létezik, false egyébként</returns>
    Task<bool> ExistsUser(string userName);

    /// <summary>
    /// Megnézi, hogy létezik-e a felhasználó és ha nem, akkor létrehozza.
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    Task EnsureUserExists(string userName);
}
