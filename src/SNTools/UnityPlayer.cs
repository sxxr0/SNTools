using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SNTools;

internal class UnityPlayer
{
    [DllImport("UnityPlayer.dll", CharSet = CharSet.Unicode)]
    public static extern int UnityMain(nint hInstance, nint hPrevInstance, string lpCmdline, int nShowCmd);

    public static int UnityMain()
    {
        var unityArgs = string.Join(' ', Environment.GetCommandLineArgs().Select(x => x.Contains(' ') ? $"\"{x}\"" : x));
        return UnityMain(Process.GetCurrentProcess().MainModule!.BaseAddress, 0, unityArgs, 1);
    }
}
