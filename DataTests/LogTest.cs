using Data.Logging;
using System.Text.Json;
namespace DataTests;

public class LogTest
{
    [Test]
    public void LoggingTest()
    {

            
        Task.Run(async () =>
        {
            ILogger logger = new FileLogger("filePath.log");
            LogData data = new LogData(new DateTime(2024, 6, 1), 1.0, 0, 1, 1);
            await Task.Run(() =>logger.LogData(data));
            while (true)
            {
                try
                {
                    using (FileStream fileStream =
                           new FileStream("filePath.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            String line = null;
                            while (line == null)
                            {
                                line = streamReader.ReadLine();
                            }
                            Assert.AreEqual(JsonSerializer.Serialize(data), line);
                        }
                    }

                    File.Delete("filePath.log");
                    break;
                }
                catch (Exception e)
                {
                }

            }
                    
                
        }).GetAwaiter().GetResult();
    }
}