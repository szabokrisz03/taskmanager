using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.DiscussionServices;

public interface ICommentService
{
    /// <summary>
    /// Komment létrehozása és adatbázisba feltöltése.
    /// </summary>
    /// <param name="taskId">Feladat egyedi azonosítója</param>
    /// <param name="userName">A kommentet író user</param>
    /// <param name="content">A komment szövege</param>
    Task CreateCommentAsync(long taskId, string userName, string content);

    /// <summary>
    /// Komment törlése.
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    /// <param name="loggedUser">Bejelentkezett felhasználó</param>
    /// <param name="commentId">Komment egyedi azonosítója</param>
    Task<List<CommentViewModel>> ListComments(long taskId);

    /// <summary>
    /// Egy feladathoz tartozó kommentek kilistázása.
    /// </summary>
    /// <param name="taskId">Feladat egyedi azonosítója</param>
    /// <returns>A kommenteket tartalmazó lista</returns>
    Task DeleteCommentAsync(string userName, string loggedUser, long commentId);
}