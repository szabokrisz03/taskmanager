namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class AzdoIterationAttributesDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public AzdoIterationTimeFrame TimeFrame { get; set; }
}

[Serializable]
public enum AzdoIterationTimeFrame
{
    current,
    future,
    past
}