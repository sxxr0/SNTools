using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace SNTools.Game;

[HarmonyPatch]
internal static class LobbyPlayerManager
{
    public static ObservableCollection<LobbyPlayerWrapper> Players { get; } = [];

    [HarmonyPatch(typeof(LobbyPlayer), LobbyPlayerAPI.UpdatePlayerInfoMethod)]
    [HarmonyPostfix]
    private static void UpdatePlayerInfoPatch([HarmonyArgument(0)] PlayerInfo playerInfo, LobbyPlayer __instance)
    {
        var existingPlayer = Players.FirstOrDefault(p => p.LobbyPlayer == __instance);
        if (existingPlayer != null)
        {
            Program.MainLogger.LogDebug("Updating player info of {playerName}", existingPlayer.DisplayName);
            existingPlayer.PlayerInfo = playerInfo;
            return;
        }

        Program.MainLogger.LogDebug("Adding lobby player {playerName}", playerInfo.GetName());
        Players.Add(new(__instance, playerInfo));
    }

    [HarmonyPatch(typeof(LobbyPlayer), "OnNetObjectDestroy")]
    [HarmonyPrefix]
    private static void OnNetObjectDestroyPatch(LobbyPlayer __instance)
    {
        var player = Players.FirstOrDefault(x => x.LobbyPlayer == __instance);
        if (player != null)
        {
            Program.MainLogger.LogDebug("Removing lobby player {playerName}", player.DisplayName);
            Players.Remove(player);
        }
    }

    [HarmonyPatch(typeof(PlayerInfo), PlayerInfoAPI.UpdateNameMethod)]
    [HarmonyPostfix]
    private static void PlayerInfoUpdateNamePatch(PlayerInfo __instance)
    {
        var player = Players.FirstOrDefault(x => x.PlayerInfo == __instance);
        player?.NotifyNameChanged();
    }

    [HarmonyPatch(typeof(PhotonNetworkProvider), PhotonNetworkProviderAPI.OnMasterClientSwitchedMethod)]
    [HarmonyPostfix]
    private static void OnMasterClientSwitchedPatch()
    {
        foreach (var player in Players)
        {
            player.NotifyMasterChanged();
        }
    }
}
