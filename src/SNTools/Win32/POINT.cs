using System.Runtime.InteropServices;

namespace SNTools.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;
}
