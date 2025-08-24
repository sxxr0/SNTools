using System.Runtime.InteropServices;

namespace SNTools.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct UNICODE_STRING
{
    public ushort Length;
    public ushort MaximumLength;
    public nint Buffer;

    public UNICODE_STRING(string text)
    {
        // Allocate unmanaged memory for the string (UTF-16, null-terminated)
        var bytes = (text.Length + 1) * 2; // +1 for null terminator
        var buffer = Marshal.StringToHGlobalUni(text);

        Length = (ushort)(text.Length * 2);       // bytes, not chars
        MaximumLength = (ushort)bytes;
        Buffer = buffer;
    }
}