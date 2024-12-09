using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.DiscussionServices;

namespace TaskManager.Srv.Components.TaskDetails;

/// <summary>
/// Kommentek megjelenítése.
/// </summary>
public partial class Discussion
{
    private string authName = "";
    private List<CommentViewModel> commentViewModels = new();
    [Parameter] public string Comment { get; set; } = "";
    [CascadingParameter(Name = "TaskId")] private long Id { get; set; }
    [CascadingParameter] private Task<AuthenticationState>? authenticationStateTask { get; set; }
    [Inject] private ICommentService commentService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var authState = await authenticationStateTask!;
        authName = authState.User.Identity?.Name ?? "";
        await ListComments();
    }

    /// <summary>
    /// Komment hozzáadása.
    /// </summary>
    public async Task AddComments()
    {
        var authState = await authenticationStateTask!;
        authName = authState.User.Identity?.Name ?? "";

        await commentService.CreateCommentAsync(Id, authName, Comment);
        await ListComments();
        Comment = "";
    }

    /// <summary>
    /// Komment törlése.
    /// - Minden felhasználó csak a saját kommentjét tudja törölni.
    /// </summary>
    /// <param name="username">Felhasználónév</param>
    /// <param name="commentView">A törlendő komment</param>
    public async Task DeleteComment(string username, CommentViewModel commentView)
    {
        var authState = await authenticationStateTask!;
        authName = authState.User.Identity?.Name ?? "";

        await commentService.DeleteCommentAsync(username, authName, commentView.RowId);
        await ListComments();
    }

    /// <summary>
    /// Kommentek listázása.
    /// </summary>
    public async Task ListComments()
    {
        commentViewModels = await commentService.ListComments(Id);
    }
}
