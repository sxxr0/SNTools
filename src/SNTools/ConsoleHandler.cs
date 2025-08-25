using SNTools.Win32;
using System.Drawing;
using System.IO;
using System.Text;

namespace SNTools;

internal static class ConsoleHandler
{
    public static bool Present { get; private set; }

    public static void Init(string title)
    {
        var stdOut = PInvoke.GetStdHandle(PInvoke.StdOutputHandle);

        if (stdOut == 0)
            return;

        Present = true;

        Console.Title = title;
        Console.OutputEncoding = Encoding.UTF8;

        if (PInvoke.GetConsoleMode(stdOut, out var outConsoleMode))
            _ = PInvoke.SetConsoleMode(stdOut, outConsoleMode | 1 | 4);
    }

    public static void NullHandles()
    {
        if (!Present)
            return;

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        PInvoke.SetStdHandle(PInvoke.StdOutputHandle, 0);
    }

    public static void SetColor(byte r, byte g, byte b)
    {
        if (!Present)
            return;

        Console.Write($"\u001b[38;2;{r};{g};{b}m");
    }

    public static void SetColor(Color color)
    {
        SetColor(color.R, color.G, color.B);
    }
}