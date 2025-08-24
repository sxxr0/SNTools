using System.Runtime.InteropServices;

namespace SNTools.Win32;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct MODULEENTRY32W
{
    public uint dwSize;
    public uint th32ModuleID;
    public uint th32ProcessID;
    public uint GlblcntUsage;
    public uint ProccntUsage;
    public nint modBaseAddr;      // BYTE* → IntPtr
    public uint modBaseSize;
    public nint hModule;          // HMODULE → IntPtr

    public fixed char szModule[256];

    public fixed char szExePath[260];
}