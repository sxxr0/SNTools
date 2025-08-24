using AppControllers;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Reflection;
using UnityExplorer;

namespace SNTools;

internal static class GameModController
{
    private static bool _inited;
    private static readonly ToolsLogger _ueLogger = new("UnityExplorer", System.Drawing.Color.Purple);

    public static Harmony Harmony { get; private set; } = null!;

    public static void Init()
    {
        if (_inited)
            return;

        _inited = true;

        Harmony = new("SNTools.GameModController");

        Harmony.Patch(AccessTools.Method(typeof(AppController), "Update"), new(((Action)OnUpdate).Method));

        Assembly.Load("UnityEngine.AssetBundleModule");
        ExplorerStandalone.CreateInstance(OnUnityExplorerLog);
    }

    private static void OnUpdate()
    {

    }

    private static void OnUnityExplorerLog(string log, UnityEngine.LogType type)
    {
        var level = type switch
        {
            UnityEngine.LogType.Error => LogLevel.Error,
            UnityEngine.LogType.Assert => LogLevel.Error,
            UnityEngine.LogType.Warning => LogLevel.Warning,
            UnityEngine.LogType.Log => LogLevel.Information,
            UnityEngine.LogType.Exception => LogLevel.Error,
            _ => LogLevel.Trace
        };

        _ueLogger.Log(level, log);
    }
}
