namespace TaskManager.Srv.Model.DTO;

/// <summary>
/// Container for listed values
/// </summary>
[Serializable]
public class AzdoListDto<TPayload>
{
    public int Count { get; set; }
    public List<TPayload> Value { get; set; } = new();
}