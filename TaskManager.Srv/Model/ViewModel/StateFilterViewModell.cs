using System.ComponentModel.DataAnnotations;

namespace TaskManager.Srv.Model.ViewModel;

public class StateFilterViewModell
{
    [Display(Name = "Igényfelmérés")]
    public bool Igeny_felmeres { get; set; } = true;
    [Display(Name = "Specifikáció alatt")]
    public bool Specifikacio_alatt { get; set; } = true;
    [Display(Name = "Fejlesztésre vár")]
    public bool Fejlesztesre_var { get; set; } = true;
    [Display(Name = "Fejlesztés alatt")]
    public bool Fejlesztes_alatt { get; set; } = true;
    [Display(Name = "Tesztelés alatt")]
    public bool Teszteles_alatt { get; set; } = true;
    [Display(Name = "Kiadásra vár")]
    public bool Kiadasra_var { get; set; } = true;
    [Display(Name = "Verziózva")]
    public bool Verziozva { get; set; } = true;
    [Display(Name = "Ajánlatadás")]
    public bool Ajanlatadas { get; set; } = true;
    [Display(Name = "Meghiúsult")]
    public bool Meghiusult { get; set; } = true;
}
