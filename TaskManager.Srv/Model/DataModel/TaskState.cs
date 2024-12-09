using System.ComponentModel.DataAnnotations;

namespace TaskManager.Srv.Model.DataModel;

/// <summary>
/// Logikát építünk a sorrendre!
/// </summary>
[Serializable]
public enum TaskState
{
    [Display(Name = "Ajánlatadás")]
    Ajanlatadas = 0,

    [Display(Name = "Igényfelmérés")]
    Igeny_felmeres,

    [Display(Name = "Specifikáció alatt")]
    Specifikacio_alatt,

    [Display(Name = "Fejlesztésre vár")]
    Fejlesztesre_var,

    [Display(Name = "Fejlesztés alatt")]
    Fejlesztes_alatt,

    [Display(Name = "Tesztelés alatt")]
    Teszteles_alatt,

    [Display(Name = "Kiadásra vár")]
    Kiadasra_var,

    [Display(Name = "Verziózva")]
    Verziozva,

    [Display(Name = "Meghiúsult")]
    Meghiusult,
}