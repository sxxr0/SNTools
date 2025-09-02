using System.Runtime.InteropServices;

namespace SNTools.Win32;

internal static unsafe class PInvoke
{
    public const uint StdInputHandle = 4294967286;
    public const uint StdOutputHandle = 4294967285;
    public const uint StdErrorHandle = 4294967284;

    [DllImport("ntdll.dll")]
    public static extern void RtlCopyUnicodeString(UNICODE_STRING* destination, UNICODE_STRING* source);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    public static extern nint GetConsoleWindow();

    [DllImport("kernel32.dll")]
    public static extern nint GetStdHandle(uint nStdHandle);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetStdHandle(uint nStdHandle, nint hHandle);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(nint hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool ClientToScreen(nint hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(nint hWnd);
}
