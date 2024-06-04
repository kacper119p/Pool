using System.Diagnostics;
using System.Text.Json;

namespace Data.Logging;

public class FileLogger : ILogger
{
    private readonly Queue<LogData> _queue;
    private readonly ManualResetEvent _hasNewItems;
    private readonly ManualResetEvent _terminate;
    private readonly WaitHandle[] _waitHandle;
    private readonly string _filePath;
    private volatile bool _terminateFlag;

    private readonly Thread _threadHandle;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
        _queue = new Queue<LogData>();
        _hasNewItems = new ManualResetEvent(false);
        _terminate = new ManualResetEvent(false);
        _waitHandle = new WaitHandle[] { _hasNewItems, _terminate };

        _threadHandle = new Thread(ProcessRequests);
        _threadHandle.IsBackground = true;
        _threadHandle.Start();
    }

    public void LogData(LogData data)
    {
        Task.Run(() =>
        {
            lock (_queue)
            {
                _queue.Enqueue(data);
            }

            _hasNewItems.Set();
        });
    }

    public void Dispose()
    {
        _terminate.Set();
        _threadHandle.Join();
    }

    private void ProcessRequests()
    {
        while (!_terminateFlag)
        {
            int i = WaitHandle.WaitAny(_waitHandle);
            _hasNewItems.Reset();

            Queue<LogData> queueCopy;
            lock (_queue)
            {
                queueCopy = new Queue<LogData>(_queue);
                _queue.Clear();
            }

            StreamWriter writer;

            while (true)
            {
                try
                {
                    writer = new StreamWriter(_filePath, true);
                    break;
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                    return;
                }
            }

            foreach (LogData data in queueCopy)
            {
                string str = JsonSerializer.Serialize(data);
                writer.WriteLine(str);
            }

            writer.Dispose();

            if (i == 1)
            {
                return;
            }
        }
    }
}
