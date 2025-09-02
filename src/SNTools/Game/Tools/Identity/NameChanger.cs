using GameModes;

namespace SNTools.Game.Tools.Identity;

public class NameChanger() : PropertyOverrideTool<string>(GameMode.MENU)
{
    protected override void RestoreDefault()
    {
        SetValue(PlatformAPI.GetUserName());
    }

    protected override void SetValue(string value)
    {
        GameContextAPI.GetLocalPlayerInfo().SetName(value ?? string.Empty);
        MenuScenery.prop_MenuScenery_0.lobbyPlayerCharacter.RefreshName();
    }

    protected override bool IsDefault()
        => string.IsNullOrEmpty(Value);
}
