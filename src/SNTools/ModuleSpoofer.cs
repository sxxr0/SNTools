using SNTools.Win32;

namespace SNTools;

internal static unsafe class ModuleSpoofer
{
    private static bool _inited;

    private static Dobby.Patch<DModule32>? _module32FirstWPatch;

    public static void Init()
    {
        if (_inited)
            return;

        _inited = true;

        _module32FirstWPatch = Dobby.CreatePatch<DModule32>("Kernel32", "Module32FirstW", OnModule32FirstW);
    }

    /// Cancelling the entire enumeration works just fine, but not very reliable long-term.
    private static bool OnModule32FirstW(nint hSnapshot, MODULEENTRY32W* lpme)
    {
        return false;
    }

    internal delegate bool DModule32(nint hSnapshot, MODULEENTRY32W* lpme);
}
