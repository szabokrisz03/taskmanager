using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Model.DTO;
using TaskManager.Srv.Services.WiServices;
using TaskManager.Srv.Utilities;
using TaskManager.Srv.Utilities.Exceptions;

namespace TaskManager.Srv.Components.TaskDetails;

/// <summary>
/// Workitemek megjelenítése.
/// </summary>
public partial class ConnectingWi
{
    private int[]? wiIdArray;
    private List<WorkItem>? workItems;
    private Dictionary<WorkItem, List<WorkItem>>? wiDetails;
    private MudNumericField<int?>? _numField;
    private Snackbar? _snackbar;
    [Parameter] public int? WiNumber { get; set; }
    [CascadingParameter(Name = "TaskId")] private long Id { get; set; }
    [Inject] public IWiService? WiService { get; set; }
    [Inject] public IWiStateService? WiStateService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ListWis();
    }

    /// <summary>
    /// Workitemek listázása.
    /// </summary>
    /// <returns></returns>
    public async Task ListWis()
    {
        wiIdArray = await WiService!.ListWorkItem(Id);
        wiDetails = WiStateService!.queryMaker(wiIdArray);

        foreach (var parentChildrenPairs in wiDetails)
        {
            parentChildrenPairs.Value.Sort();

            var pointerWi = GetChildrenWi(parentChildrenPairs.Value);

            if (pointerWi != null)
            {
                WiStateNameChanger(pointerWi, parentChildrenPairs.Key);
            }
            else
            {
                parentChildrenPairs.Key.ClearState = "Ütemezésre vár";
            }
        }

        foreach (var key in wiDetails.Keys)
        {
            workItems ??= new();

            workItems.Add(key);
        }
    }

    /// <summary>
    /// Sorbarendezett workitemek közül adja vissza a státusz szerint a legkedvezőbbet az alábbi szerint:
    /// - Sorrendben a legkésőbbi "InProgress"
    /// - Ha nincs, akkor a legutolsó "Done" utáni első "ToDo"
    /// - Ha nincs, a legutolsó "Done"
    /// </summary>
    /// <param name="sortedWis">Sorbarendezett workitemeket tartalmazó lista</param>
    /// <returns>A megtalált workitem</returns>
    /// <exception cref="NonFatalException"></exception>
    public WorkItem? GetChildrenWi(List<WorkItem> sortedWis)
    {
        if (sortedWis.Count <= 0)
        {
            return null;
        }

        var wi = sortedWis.Where(x => x.State == "In Progress").LastOrDefault();

        if (wi is null)
        {
            var lastDone = sortedWis.Where(x => x.State == "Done").LastOrDefault();

            wi = lastDone != null
                ? sortedWis.Where(x => (x.State == "To Do" || x.State == "New") && (x.CompareTo(lastDone) > 0)).FirstOrDefault()
                : sortedWis.Where(x => x.State == "To Do" || x.State == "New").FirstOrDefault();

            wi ??= sortedWis.Where(x => x.State == "Done").LastOrDefault();
        }

        return wi is null ? throw new NonFatalException("Nem lett lekezelve az összes státusz!") : wi;
    }

    /// <summary>
    /// Típus és státusz szerint nevezi át a workitemek státuszát.
    /// </summary>
    /// <param name="wi">A linkelt workitem</param>
    /// <param name="item">A változtatandó workitem</param>
    public void WiStateNameChanger(WorkItem wi, WorkItem item)
    {
        if (wi.Type == "Rendszerszervezési feladat")
        {
            switch (wi.State)
            {
                case "To Do":
                    item.ClearState = "Specifikációra vár";
                    break;
                case "In Progress":
                    item.ClearState = "Specifikáció alatt";
                    break;
                case "Done":
                    item.ClearState = "Specifikáció kész";
                    break;
            }
        }

        if (wi.Type == "Fejlesztési feladat")
        {
            switch (wi.State)
            {
                case "To Do":
                    item.ClearState = "Fejlesztésre vár";
                    break;
                case "In Progress":
                    item.ClearState = "Fejlesztés alatt";
                    break;
                case "Done":
                    item.ClearState = "Fejlesztés kész";
                    break;
            }
        }

        if (wi.Type == "Technikai változásjelentés")
        {
            switch (wi.State)
            {
                case "To Do":
                    item.ClearState = "Tesztelésre vár";
                    break;
                case "In Progress":
                    item.ClearState = "Tesztelés alatt";
                    break;
                case "Kiadásra vár":
                    item.ClearState = "Kiadásra vár";
                    break;
                case "Done":
                    item.ClearState = "Kiadva";
                    break;
            }
        }

        if (wi.Type == "Hibajegy")
        {
            switch (wi.State)
            {
                case "New":
                    item.ClearState = "Hibajavításra vár";
                    break;
                case "In Progress":
                    item.ClearState = "Hibajavítás alatt";
                    break;
                case "Done":
                    item.ClearState = "Hibajavítás kész";
                    break;
            }
        }
    }

    /// <summary>
    /// Létrehozza a különböző snackbar-okat.
    /// </summary>
    /// <param name="snackBarId">SnackBarConstból érkező érték</param>
    public void addSnackBar(int snackBarId)
    {
        switch (snackBarId)
        {
            case 0:
                _snackbar = Snackbar.Add("Nem adtál meg workitem ID-t!", Severity.Warning, configure =>
                {
                    configure.VisibleStateDuration = 3000;
                    configure.HideTransitionDuration = 200;
                    configure.ShowTransitionDuration = 200;
                    configure.ShowCloseIcon = true;
                });
                break;
            case 1:
                _snackbar = Snackbar.Add("A megadott workitem már hozzá van adva a feladathoz!", Severity.Warning, configure =>
                {
                    configure.VisibleStateDuration = 3000;
                    configure.HideTransitionDuration = 200;
                    configure.ShowTransitionDuration = 200;
                    configure.ShowCloseIcon = true;
                });
                break;
            case 2:
                _snackbar = Snackbar.Add("Csak igény típusú workitemet lehet felvenni!", Severity.Warning, configure =>
                {
                    configure.VisibleStateDuration = 3000;
                    configure.HideTransitionDuration = 200;
                    configure.ShowTransitionDuration = 200;
                    configure.ShowCloseIcon = true;
                });
                break;
            case 3:
                _snackbar = Snackbar.Add("Sikeres hozzáadás!", Severity.Success, configure =>
                {
                    configure.VisibleStateDuration = 3000;
                    configure.HideTransitionDuration = 200;
                    configure.ShowTransitionDuration = 200;
                    configure.ShowCloseIcon = true;
                });
                break;
            case 4:
                _snackbar = Snackbar.Add("A megadott workitem nem létezik!", Severity.Warning, configure =>
                {
                    configure.VisibleStateDuration = 3000;
                    configure.HideTransitionDuration = 200;
                    configure.ShowTransitionDuration = 200;
                    configure.ShowCloseIcon = true;
                });
                break;
        }

        _numField!.Reset();
    }

    /// <summary>
    /// Workitem hozzáadása a feladathoz.
    /// </summary>
    public async Task AddWi()
    {
        if (WiNumber == null)
        {
            addSnackBar(SnackBarConst.WI_NUMBER_NULL);
            return;
        }

        foreach (var item in wiDetails!.Keys)
        {
            if (item.Id == WiNumber)
            {
                addSnackBar(SnackBarConst.WI_NUMBER_EXIST);
                return;
            }
        }

        int[] wiId = new int[] { WiNumber.Value };
        List<WorkItem> parentWi = new();
        WiStateService!.PropertyWis(wiId, parentWi);

        if (parentWi.Count <= 0)
        {
            addSnackBar(SnackBarConst.WI_NUMBER_NOT_EXIST);
            return;
        }

        if (parentWi.First().Type != "Igény")
        {
            addSnackBar(SnackBarConst.WI_NUMBER_WRONG_TYPE);
            return;
        }

        WiService?.CreateWiAsync(WiNumber.Value, Id);
        addSnackBar(SnackBarConst.WI_NUMBER_SUCCESS);
        await ListWis();
    }

    public async Task deleteConnectingWi(int id)
    {
        await WiService!.DeleteWi(id);
        await ListWis();
    }

    /// <summary>
    /// Lenyíló menüért felelős.
    /// </summary>
    /// <param name="id">A lenyitandó workitem</param>
    public void ShowConnectingWi(int id)
    {
        foreach (var item in workItems!)
        {
            item.IsOpen = item.Id == id && !item.IsOpen;
        }
    }
}