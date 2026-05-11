namespace ConcurrentProgramming.Data.Tests;

public class BackgroundFileLoggerTests
{
    [Fact]
    private async Task BackgroundFileLoggerDeletesOldFile()
    {
        const string testFilePath = nameof(BackgroundFileLoggerDeletesOldFile);
        
        await File.WriteAllTextAsync(testFilePath, "test");

        var cancelImmediatelyTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0.1));
        var logger = new BackgroundFileLogger(testFilePath);
        await logger.LoggingThread(cancelImmediatelyTokenSource.Token);
        
        Assert.False(File.Exists(testFilePath));
    }

    private record TestRecord(string Text);
    
    [Fact]
    private async Task BackgroundFileLoggerSerializesAsJson()
    {
        const string testFilePath = nameof(BackgroundFileLoggerSerializesAsJson);
        await File.WriteAllTextAsync(testFilePath, "test");

        var loggerCancellationSource = new CancellationTokenSource();
        var logger = new BackgroundFileLogger(testFilePath);
        _ = Task.Run(() => logger.LoggingThread(loggerCancellationSource.Token), loggerCancellationSource.Token);
        
        logger.Log(new TestRecord("Hi"));
        
        await Task.Delay(100);
        await loggerCancellationSource.CancelAsync();
        
        Assert.EndsWith("[TestRecord] {\"Text\":\"Hi\"}\n", File.ReadAllText(testFilePath));
        
        // We've now canceled the logging thread, this should not get logged.
        logger.Log(new TestRecord("Other"));
        await Task.Delay(100);
        Assert.EndsWith("[TestRecord] {\"Text\":\"Hi\"}\n", File.ReadAllText(testFilePath));
    }
}
