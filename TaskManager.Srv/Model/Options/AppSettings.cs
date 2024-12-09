namespace TaskManager.Srv.Model.Options;

[Serializable]
public class AppSettings
{
    public Remotes Remotes { get; set; } = new();
    public int CacheItemLimit { get; set; } = 256;
}