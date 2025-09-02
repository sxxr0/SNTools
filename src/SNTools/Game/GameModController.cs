using AppControllers;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using SNTools.UI;
using System.Reflection;
using UnityExplorer;

namespace SNTools.Game;

[HarmonyPatch]
public static class GameModController
{
    private static bool _inited;
    private static readonly ToolsLogger _ueLogger = new("UnityExplorer", System.Drawing.Color.Purple);

    public static Harmony Harmony { get; private set; } = null!;
    public static GameMode CurrentGameMode { get; private set; }

    public static event Action<GameMode>? GameModeChanged;
    public static event Action<GameMode>? GameModeChanging;

    public static void Init()
    {
        if (_inited)
            return;

        _inited = true;

        Harmony = new("SNTools");

        Harmony.PatchAll(typeof(GameModController).Assembly);

        Assembly.Load("UnityEngine.AssetBundleModule");
        ExplorerStandalone.CreateInstance(OnUnityExplorerLog);
    }

    [HarmonyPatch(typeof(AppController), "Start")]
    [HarmonyPrefix]
    private static void OnStart()
    {
        ToolsUIController.InitGameOverlay();
    }

    [HarmonyPatch(typeof(AppController), "Update")]
    [HarmonyPrefix]
    private static void OnUpdate()
    {
        ToolsUIController.Update();
    }

    [HarmonyPatch(typeof(GameModeController), GameModeControllerAPI.SetGameModeMethod)]
    [HarmonyPrefix]
    private static void OnSettingGameMode([HarmonyArgument(0)] GameMode gameMode)
    {
        if (gameMode == CurrentGameMode)
            return;

        GameModeChanging?.Invoke(gameMode);
    }

    [HarmonyPatch(typeof(GameModeController), GameModeControllerAPI.SetGameModeMethod)]
    [HarmonyPostfix]
    private static void OnGameModeSet([HarmonyArgument(0)] GameMode gameMode)
    {
        if (gameMode == CurrentGameMode)
            return;

        CurrentGameMode = gameMode;

        GameModeChanged?.Invoke(gameMode);
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
