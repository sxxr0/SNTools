using HarmonyLib;
using Il2CppSystem.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace SNTools.Game;

// For some reason, Steam requires manual room list refreshes, while GDK automatically sends incremental room updates

[HarmonyPatch]
public static class RoomManager
{
    public static ObservableCollection<RoomWrapper> Rooms { get; } = [];

#if GDK
    static RoomManager()
    {
        GameModController.GameModeChanged += OnGameModeChanged;
    }

    private static void OnGameModeChanged(GameMode gameMode)
    {
        // When we enter a room on GDK, the room list is not cleared.
        // This means once we leave the room, the old rooms will still be present in the list.
        if (gameMode == GameMode.LOBBY)
            Rooms.Clear();
    }
#endif

    public static void RefreshList()
    {
#if STEAM
        PhotonNetworkAPI.GetCustomRoomList();
#endif
    }

    [HarmonyPatch(typeof(PhotonNetworkProvider), PhotonNetworkProviderAPI.OnRoomListUpdateMethod)]
    [HarmonyPostfix]
    private static void OnRoomListUpdatePatch([HarmonyArgument(0)] Il2CppSystem.Collections.Generic.List<PhotonRoomInfo> rooms, PhotonNetworkProvider __instance)
    {
#if STEAM
        var enumerableRooms = rooms.Cast<Il2CppSystem.Collections.Generic.IEnumerable<PhotonRoomInfo>>();

        for (var i = 0; i < Rooms.Count; i++)
        {
            if (!enumerableRooms.Any((Func<PhotonRoomInfo, bool>)(x => x.prop_String_0 == Rooms[i].Id)))
            {
                Rooms.RemoveAt(i);
                i--;
            }
        }
#endif

        foreach (var room in rooms)
        {
#if GDK
            if (room.GetPlayerCount() == 0)
            {
                var toRemove = Rooms.FirstOrDefault(x => x.Id == room.GetId());
                if (toRemove != null)
                {
                    Program.MainLogger.LogDebug("Removing room: {id}", toRemove.Id);
                    Rooms.Remove(toRemove);
                }

                continue;
            }
#endif

            var holoNetRoom = __instance.ConvertRoomToHoloNet(room);

            var existingWrapper = Rooms.FirstOrDefault(x => x.Id == room.GetId());
            if (existingWrapper == null)
            {
                Program.MainLogger.LogDebug("Adding new room: {id}", room.GetId());
                Rooms.Add(new(holoNetRoom));
            }
            else
            {
                existingWrapper.HoloNetRoom = holoNetRoom;
            }
        }
    }
}
