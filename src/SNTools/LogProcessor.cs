using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Globalization;

namespace SNTools;

public static partial class LogProcessor
{
    private static readonly object _logLock = new();

    private static readonly string _colorTagStart = "<color=";
    private static readonly string _colorResetTag = "</color>";

    /// <summary>
    /// Logs a message to the console (with color formatting) and log files (with color formatting discarded).
    /// </summary>
    /// <remarks>
    /// This also prepends the log time and category name (if present).
    /// </remarks>
    public static void Log(string message, LogLevel level, string? category, Color categoryColor)
    {
        var time = DateTime.Now;

        lock (_logLock)
        {
            var baseColor = level switch
            {
                LogLevel.Trace => Color.LightCyan,
                LogLevel.Debug => Color.DarkGray,
                LogLevel.Information => Color.LightGray,
                LogLevel.Warning => Color.Yellow,
                LogLevel.Error => Color.Red,
                LogLevel.Critical => Color.DarkRed,
                _ => Color.White
            };

            ConsoleHandler.SetColor(baseColor);
            Console.Write('[');
            ConsoleHandler.SetColor(Color.MediumPurple);
            Console.Write(time.ToString("HH:mm:ss.fff"));
            ConsoleHandler.SetColor(baseColor);
            Console.Write("] ");

            var messageOffset = 15;

            if (category != null)
            {
                Console.Write('[');
                ConsoleHandler.SetColor(categoryColor);
                Console.Write(category);
                ConsoleHandler.SetColor(baseColor);
                Console.Write("] ");

                messageOffset += 3 + category.Length;
            }

            var lastIndex = 0;

            for (var i = 0; i < message.Length; i++)
            {
                var span = message.AsSpan(i);

                void WritePart(int skipLength)
                {
                    Console.Out.Write(message.AsSpan(lastIndex, i - lastIndex));
                    i += skipLength - 1;
                    lastIndex = i + 1;
                }

                if (span[0] == '\n')
                {
                    WritePart(1);

                    Console.Write('\n');
                    for (var i2 = 0; i2 < messageOffset; i2++)
                        Console.Write(' ');
                }
                else if (span.StartsWith(_colorResetTag))
                {
                    WritePart(_colorResetTag.Length);

                    ConsoleHandler.SetColor(baseColor);
                }
                else if (span.StartsWith(_colorTagStart))
                {
                    var closingIdx = span[_colorTagStart.Length..].IndexOf('>');
                    if (closingIdx == -1)
                        continue;

                    var value = span.Slice(_colorTagStart.Length, closingIdx);
                    if (value[0] == '#')
                    {
                        if (value.Length != 7
                            || !byte.TryParse(value.Slice(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r)
                            || !byte.TryParse(value.Slice(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g)
                            || !byte.TryParse(value.Slice(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
                            continue;

                        WritePart(_colorTagStart.Length + closingIdx + 1);

                        ConsoleHandler.SetColor(r, g, b);
                    }
                    else
                    {
                        if (!Enum.TryParse<KnownColor>(value, true, out var knownColor))
                            continue;

                        WritePart(_colorTagStart.Length + closingIdx + 1);

                        ConsoleHandler.SetColor(Color.FromKnownColor(knownColor));
                    }
                }
            }

            if (lastIndex < message.Length)
                Console.Out.Write(message.AsSpan(lastIndex));

            ConsoleHandler.SetColor(baseColor);

            Console.WriteLine();
        }
    }
}
