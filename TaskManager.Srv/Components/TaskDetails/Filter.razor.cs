using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.Reflection;

using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Components.TaskDetails;

public partial class Filter
{
    private bool isOpen = false;
    [Parameter] public EventCallback<StateFilterViewModell> OnClick { get; set; }
    [CascadingParameter(Name = "FilterView")] private StateFilterViewModell? StateFilterViewModell { get; set; }

    private async Task CheckedFilterChanged()
    {
        isOpen = !isOpen;
        await OnClick.InvokeAsync(StateFilterViewModell);
    }

    private async Task OpenFilter() => isOpen = !isOpen;
}
