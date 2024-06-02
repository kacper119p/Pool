using System.Text.Json;

namespace Data.Logging;

public class FileLogger : ILogger
{
    private readonly Queue<LogData> _queue;
    private readonly ManualResetEvent _hasNewItems = new ManualResetEvent(false);
    private readonly ManualResetEvent _terminate;
    private readonly ManualResetEvent _waiting;
    private readonly WaitHandle[] _waitHandle;
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
        _queue = new Queue<LogData>();
        _hasNewItems = new ManualResetEvent(false);
        _terminate = new ManualResetEvent(false);
        _waiting = new ManualResetEvent(false);
        _waitHandle = new WaitHandle[] { _hasNewItems, _terminate };
        
        Thread threadHandle = new Thread(ProcessRequests);
        threadHandle.Start();

       
    }

    public void LogData(LogData data)
    {
        lock (_queue)
        {
            _queue.Enqueue(data);
        }

        _hasNewItems.Set();
    }

    public void Dispose()
    {
        _terminate.Set();
    }

    private void ProcessRequests()
    {
        while (true)
        {
            _waiting.Set();
            int i = WaitHandle.WaitAny(_waitHandle);
            _hasNewItems.Reset();
            _waiting.Reset();

            Queue<LogData> queueCopy;
            lock (_queue)
            {
                queueCopy = new Queue<LogData>(_queue);
                _queue.Clear();
            }

            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                foreach (LogData data in queueCopy)
                {
                    string str = JsonSerializer.Serialize(data);
                    writer.WriteLine(str);
                }
            }

            if (i == 1)
            {
                return;
            }
        }
    }
}
