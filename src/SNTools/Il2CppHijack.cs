using Il2CppInterop.Common;
using Il2CppInterop.HarmonySupport;
using Il2CppInterop.Runtime.Startup;
using SNTools.Il2CppInteropHelpers;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SNTools;

internal static class Il2CppHijack
{
    private static bool _inited;

    private static Dobby.Patch<DIl2CppRuntimeInvoke>? _il2cppRuntimeInvokePatch;

    public static event Action? ReadyToMod;

    public static bool Init()
    {
        if (_inited)
            return true;

        var gameAssemblyPath = Path.Combine(Program.GameDir, "GameAssembly.dll");
        if (!NativeLibrary.TryLoad(gameAssemblyPath, out var gameAssemblyHandle))
            return false;

        _il2cppRuntimeInvokePatch = Dobby.CreatePatch<DIl2CppRuntimeInvoke>(gameAssemblyHandle, "il2cpp_runtime_invoke", OnIl2CppRuntimeInvoke);

        _inited = true;
        return true;
    }

    private static nint OnIl2CppRuntimeInvoke(nint method, nint obj, nint args, nint exc)
    {
        if (_il2cppRuntimeInvokePatch == null)
            return 0;

        var result = _il2cppRuntimeInvokePatch.Original(method, obj, args, exc);

        var name = method == 0 ? null : Marshal.PtrToStringUTF8(il2cpp_method_get_name(method));
        if (name == null || !name.Contains("Internal_ActiveSceneChanged"))
            return result;

        _il2cppRuntimeInvokePatch.Destroy();

        OnFirstSceneLoad();

        return result;
    }

    private static void OnFirstSceneLoad()
    {
        Il2CppInteropRuntime.Create(new()
        {
            DetourProvider = new Il2CppInteropDetourProvider(),
            UnityVersion = new Version(2022, 3, 14)
        }).AddLogger(new ToolsLogger("Il2CppInterop", Color.Cyan))
          .AddHarmonySupport()
          .Start();

        ReadyToMod?.Invoke();
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate nint DIl2CppRuntimeInvoke(nint method, nint obj, nint args, nint exc);

    // I initially wanted to use the import from Il2CppInterop, but they have a static constructor which cannot run before the Il2CppInteropRuntime is initialized.
    [DllImport("GameAssembly", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint il2cpp_method_get_name(nint method);
}
