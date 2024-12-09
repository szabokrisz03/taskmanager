namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class AzdoProjectDto : AzureDtoBase
{
    public string Name { get; set; } = "";
    public string State { get; set; } = "";
}