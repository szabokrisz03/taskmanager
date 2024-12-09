namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class WIQLResponse
{
    public string QueryType { get; set; } = "";
    public string QueryResultType { get; set; } = "";
    public List<WIQLResponseBodyColumns> Columns { get; set; } = new();
    public List<WIQLResponseBodyWIs> WorkItems { get; set; } = new();
    public List<Wi> workItemRelations { get; set; } = new();
}

[Serializable]
public class WIQLResponseBodyColumns
{
    public string ReferenceName { get; set; } = "";
    public string Name { get; set; } = "";
    public string Url { get; set; } = "";
}

[Serializable]
public class WIQLResponseBodyWIs
{
    public int Id { get; set; }
    public string Url { get; set; } = "";
}

[Serializable]
public class Wi
{
    public string rel { get; set; } = "";
    public TargetWis? target { get; set; }
}

[Serializable]
public class TargetWis
{
    public int id { get; set; }
    public string url { get; set; } = "";
}