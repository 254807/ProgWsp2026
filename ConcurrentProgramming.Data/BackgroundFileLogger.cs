using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data;

/// <summary>
/// A logger which writes to a file in a background thread.
/// </summary>
/// <param name="filePath">The path to the file to write to.</param>
public sealed class BackgroundFileLogger(string filePath) : ILogger
{
    private readonly BlockingCollection<(object Message, DateTime Time)> _queue = [];

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
            try
            {
                var (message, time) = _queue.Take(token);
                var json = JsonSerializer.Serialize(message, _jsonSerializerOptions);

                await File.AppendAllTextAsync(filePath, $"[{time}] [{message.GetType().Name}] {json}\n", token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to append to log file: {filePath}: {ex}");
            }
        }
    }
    
    public void Log<T>(T? message)
    {
        if (message is null)
            return;
        
        _queue.Add((message, DateTime.Now));
    }
}
