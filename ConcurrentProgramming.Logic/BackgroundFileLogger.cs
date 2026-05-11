using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Logic;

/// <summary>
/// A logger which writes to a file in a background thread.
/// </summary>
/// <param name="filePath">The path to the file to write to.</param>
public sealed class BackgroundFileLogger(string filePath) : ILogger
{
    private readonly BlockingCollection<object> _queue = [];

    private readonly JsonSerializerOptions _jsonSerializerOptions = new();
    
    /// <summary>
    /// Begins the thread that performs logging.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    public async Task LoggingThread(CancellationToken token)
    {
        try
        {
            File.Delete(filePath);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Failed to remove log file: {filePath}: {ex}");
            return;
        }
        
        while (!token.IsCancellationRequested)
        {
            var message = _queue.Take(token);
            
            try
            {
                await using var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                await using var textWriter = new StreamWriter(fileStream);
                fileStream.Seek(0, SeekOrigin.End);

                var json = JsonSerializer.Serialize(message, _jsonSerializerOptions);

                await textWriter.WriteLineAsync($"[{message.GetType()}] {json}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to append to log file: {filePath}: {ex}");
            }
        }
    }
    
    public void Log<T>(T? message)
    {
        if (message is null)
            return;
        
        _queue.Add(message);
    }
}
