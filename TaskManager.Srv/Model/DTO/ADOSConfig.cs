namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class ADOSConfig
{
    public string ApiVer { get; set; } = "";
    public string Host { get; set; } = "";
    public string Collection { get; set; } = "";
    public string TeamProject { get; set; } = "";
}