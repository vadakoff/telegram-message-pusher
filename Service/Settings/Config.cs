namespace Service.Settings;

public class Config
{
    public string ConnectionStrings { init; get; }
    public int LoopIntervalSeconds { init; get; }
    public string ChatId { init; get; }
    public string AccessToken { init; get; }
}