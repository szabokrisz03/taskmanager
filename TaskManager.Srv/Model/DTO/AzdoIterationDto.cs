namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class AzdoIterationDto : AzureDtoBase
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public AzdoIterationAttributesDto Attributes { get; set; } = new();
}