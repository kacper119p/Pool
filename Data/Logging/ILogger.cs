namespace Data.Logging;

public interface ILogger : IDisposable
{
    public void LogData(LogData data);
}
