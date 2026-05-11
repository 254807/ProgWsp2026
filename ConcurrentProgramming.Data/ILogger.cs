namespace ConcurrentProgramming.Data;

/// <summary>
/// Allows for logging messages.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Logs the message, serialized as json.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Log<T>(T message);
}
