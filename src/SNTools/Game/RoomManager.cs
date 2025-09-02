using HarmonyLib;
using Il2CppSystem.Linq;
using System.Collections.ObjectModel;

namespace SNTools.Game;

[HarmonyPatch]
public static class RoomManager
{
    public static ObservableCollection<RoomWrapper> Rooms { get; } = [];

    public static void RefreshList()
    {
        PhotonNetworkAPI.GetCustomRoomList();
    }

    [HarmonyPatch(typeof(PhotonNetworkProvider), PhotonNetworkProviderAPI.OnRoomListUpdateMethod)]
    [HarmonyPostfix]
    private static void OnGetRoomList([HarmonyArgument(0)] Il2CppSystem.Collections.Generic.List<PhotonRoomInfo> rooms, PhotonNetworkProvider __instance)
    {
        var enumerableRooms = rooms.Cast<Il2CppSystem.Collections.Generic.IEnumerable<PhotonRoomInfo>>();

        for (var i = 0; i < Rooms.Count; i++)
        {
            if (!enumerableRooms.Any((Func<PhotonRoomInfo, bool>)(x => x.prop_String_0 == Rooms[i].Id)))
            {
                Rooms.RemoveAt(i);
                i--;
            }
        }

        foreach (var room in rooms)
        {
            var holoNetRoom = __instance.ConvertRoomToHoloNet(room);

            var existingWrapper = Rooms.FirstOrDefault(x => x.Id == room.prop_String_0);
            if (existingWrapper == null)
                Rooms.Add(new(holoNetRoom));
            else
                existingWrapper.HoloNetRoom = holoNetRoom;
        }
    }
}
