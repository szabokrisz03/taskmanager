using System.ComponentModel.DataAnnotations;

namespace TaskManager.Srv.Model.DataModel;

/// <summary>
/// Logik�t �p�t�nk a sorrendre!
/// </summary>
[Serializable]
public enum TaskState
{
    [Display(Name = "Aj�nlatad�s")]
    Ajanlatadas = 0,

    [Display(Name = "Ig�nyfelm�r�s")]
    Igeny_felmeres,

    [Display(Name = "Specifik�ci� alatt")]
    Specifikacio_alatt,

    [Display(Name = "Fejleszt�sre v�r")]
    Fejlesztesre_var,

    [Display(Name = "Fejleszt�s alatt")]
    Fejlesztes_alatt,

    [Display(Name = "Tesztel�s alatt")]
    Teszteles_alatt,

    [Display(Name = "Kiad�sra v�r")]
    Kiadasra_var,

    [Display(Name = "Verzi�zva")]
    Verziozva,

    [Display(Name = "Meghi�sult")]
    Meghiusult,
}