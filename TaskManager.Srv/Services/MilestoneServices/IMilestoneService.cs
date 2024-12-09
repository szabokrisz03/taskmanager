using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.MilestoneServices;

public interface IMilestoneService
{
    /// <summary>
    /// Határidő lezárása teljesítettre. 
    /// </summary>
    /// <param name="milestoneId">Határidő egyedi azonosítója</param>
    Task<List<MilestoneViewModel>> ListMilestones(long TaskId);

    /// <summary>
    /// Határidő törlése.
    /// </summary>
    /// <param name="milestoneId">Határidő egyedi azonosítója</param>
    Task<MilestoneViewModel> CreateMilestone(MilestoneViewModel milestoneView);

    /// <summary>
    /// Határidő módosítása adatbázisban
    /// </summary>
    /// <param name="model">A módosítandó határidő viewModelje</param>
    void UpdateMilestonekDbSync(MilestoneViewModel model);

    /// <summary>
    /// Meghívja az UpdateMilestonekDbSync metódust.
    /// </summary>
    /// <param name="model">A módosítandó határidő viewModelje</param>
    Task UpdateMilestonekDb(MilestoneViewModel model);

    /// <summary>
    /// Megszámolja, hogy az adott feladathoz hány darab határidő tartozik
    /// </summary>
    /// <param name="taskId">A feladat id-je</param>
    /// <returns>A határidők darabszáma</returns>
    Task<int> CountTaskMilestone(long taskId);

    /// <summary>
    /// Egy feladathoz tartozó határidők kilistázása.
    /// </summary>
    /// <param name="TaskId">Feladat egyedi azonosítója</param>
    /// <returns>Határidőket tartalmazó lista</returns>
    Task CloseMilestone(long milestoneId);

    /// <summary>
    /// Határidő létrehozása és adatbázisba feltöltése.
    /// </summary>
    /// <param name="milestoneView"></param>
    /// <returns>A hozzáadott határidő</returns>
    Task DeleteMilestone(long milestoneId);
}