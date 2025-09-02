using GameModes;

namespace SNTools.Game.Features.Identity;

internal static class NameChanger
{
    public static string Name
    {
        get => GameContextAPI.GetLocalPlayerInfo().GetName();
        set
        {
            GameContextAPI.GetLocalPlayerInfo().SetName(value);
            MenuScenery.prop_MenuScenery_0.lobbyPlayerCharacter.RefreshName();
        }
    }

    public static void RestoreDefaultName()
    {
        Name = PlatformAPI.GetUserName();
    }
}
