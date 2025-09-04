using SNTools.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SNTools;

internal static unsafe class ExeSpoofer
{
    private static readonly nint _mainModuleHandle = Process.GetCurrentProcess().MainModule!.BaseAddress;
    private static UNICODE_STRING* _fakeExePath;
    private static bool _inited;

    private static Dobby.Patch<DLdrGetDllFullName>? _ldrGetDllFullNamePatch;

    public static void Init()
    {
        if (_inited)
            return;

        _inited = true;

        _fakeExePath = (UNICODE_STRING*)Marshal.AllocHGlobal(sizeof(UNICODE_STRING));
        *_fakeExePath = new(Program.GameExePath);

        _ldrGetDllFullNamePatch = Dobby.CreatePatch<DLdrGetDllFullName>("ntdll", "LdrGetDllFullName", OnLdrGetDllFullName);
    }

    public static void Dispose()
    {
        _ldrGetDllFullNamePatch?.Destroy();
        _ldrGetDllFullNamePatch = null;
    }

    private static uint OnLdrGetDllFullName(nint hModule, UNICODE_STRING* lpFilename)
    {
        if (hModule == 0 || hModule == _mainModuleHandle)
        {
            PInvoke.RtlCopyUnicodeString(lpFilename, _fakeExePath);

            if (lpFilename->MaximumLength < _fakeExePath->Length + sizeof(char))
                return 0xC0000023; // STATUS_BUFFER_TOO_SMALL

            return 0;
        }

        return _ldrGetDllFullNamePatch == null ? 0 : _ldrGetDllFullNamePatch.Original(hModule, lpFilename);
    }

    internal delegate uint DLdrGetDllFullName(nint hModule, UNICODE_STRING* lpFilename);
}
