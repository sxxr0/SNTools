using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.IO;

namespace SNTools.Game;

[HarmonyPatch]
internal static class LocalLoadout
{
    private static readonly string _loadoutSavePath = Path.Combine(Program.AppDataDir, "Loadout.json");

    [HarmonyPrepare]
    private static void ApplyPatches()
    {
        GameModController.Harmony.PatchObfuscated(typeof(PlayfabBackendAdapter), PlayfabBackendAdapterAPI.UpdateLoadoutRequestMethod, new(AccessTools.Method(typeof(LocalLoadout), nameof(UpdateLoadoutRequestPatch))));
        GameModController.Harmony.PatchObfuscated(typeof(PlayfabBackendAdapter), PlayfabBackendAdapterAPI.GetLoadoutRequestMethod, new(AccessTools.Method(typeof(LocalLoadout), nameof(GetLoadoutRequestPatch))));
    }

    private static bool GetLoadoutRequestPatch([HarmonyArgument(0)] Il2CppSystem.Action<GetLoadoutRequestResult> callback, ref GetLoadoutRequestResult __result)
    {
        Program.MainLogger.LogInformation("Loading local loadout");

        var loadout = LoadLocalLoadout();
        if (loadout == null)
        {
            Program.MainLogger.LogInformation("No local loadout found. Requesting loadout from PlayFab...");
            return true;
        }

        __result = new();
        __result.SetLoadoutResult(loadout, callback);

        return false;
    }

    private static bool UpdateLoadoutRequestPatch([HarmonyArgument(0)] Loadout loadout, [HarmonyArgument(1)] Il2CppSystem.Action<UpdateLoadoutRequestResult> callback, ref UpdateLoadoutRequestResult __result)
    {
        Program.MainLogger.LogInformation("Doing a local loadout save");

        SaveLoadoutLocally(loadout);

        __result = new();
        __result.SetResultReceived(callback);

        return false;
    }

    public static Loadout? LoadLocalLoadout()
    {
        string loadoutJson;
        try
        {
            loadoutJson = File.ReadAllText(_loadoutSavePath);
        }
        catch
        {
            return null;
        }

        LoadoutSerializer? loadoutSerializer = null;
        try
        {
            loadoutSerializer = UnityEngine.JsonUtility.FromJson<LoadoutSerializer>(loadoutJson);
        }
        catch { }

        if (loadoutSerializer == null)
        {
            try
            {
                File.Delete(_loadoutSavePath);
            }
            catch { }

            return null;
        }

        return loadoutSerializer.Unbox();
    }

    public static void SaveLoadoutLocally(Loadout loadout)
    {
        var serializedLoadout = UnityEngine.JsonUtility.ToJson(loadout.Box());
        Directory.CreateDirectory(Path.GetDirectoryName(_loadoutSavePath)!);
        File.WriteAllText(_loadoutSavePath, serializedLoadout);

        Program.MainLogger.LogInformation("Loadout saved locally");
    }
}
