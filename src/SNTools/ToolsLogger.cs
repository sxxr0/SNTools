using Microsoft.Extensions.Logging;
using System.Drawing;

namespace SNTools;

public class ToolsLogger(string category, Color categoryColor) : ILogger
{
    public string Category { get; } = category;

    public Color CategoryColor { get; } = categoryColor;

    IDisposable ILogger.BeginScope<TState>(TState state) => null!;

    bool ILogger.IsEnabled(LogLevel logLevel) => logLevel is > LogLevel.Trace and not LogLevel.None;

    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel is <= LogLevel.Trace or LogLevel.None)
            return;

        var msg = formatter(state, exception);

        LogProcessor.Log(msg, logLevel, Category, CategoryColor);
    }
}
