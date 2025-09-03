namespace SNTools.Game;

public class LobbyPlayerWrapper(LobbyPlayer lobbyPlayer, PlayerInfo playerInfo)
{
    public LobbyPlayer LobbyPlayer { get; } = lobbyPlayer;

    public PlayerInfo PlayerInfo { get; set; } = playerInfo;

    public string DisplayName => PlayerInfo.GetName();

    public string UnreliableId => PlayerInfo.GetID();

    public int LobbySlot => LobbyPlayer.field_Public_LobbyCharacter_0?._lobbyPosition ?? -1;

    public bool IsLocal => LobbyPlayer.prop_HoloNetObject_0.IsLocal;
}
