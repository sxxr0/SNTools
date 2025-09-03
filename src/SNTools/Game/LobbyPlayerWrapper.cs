using CommunityToolkit.Mvvm.ComponentModel;

namespace SNTools.Game;

public partial class LobbyPlayerWrapper(LobbyPlayer lobbyPlayer, PlayerInfo playerInfo) : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayName))]
    [NotifyPropertyChangedFor(nameof(UnreliableId))]
    public PlayerInfo _playerInfo = playerInfo;

    public LobbyPlayer LobbyPlayer { get; } = lobbyPlayer;

    public string DisplayName => PlayerInfo.GetName();

    public string UnreliableId => PlayerInfo.GetID();

    public int LobbySlot => LobbyPlayer.field_Public_LobbyCharacter_0?._lobbyPosition ?? -1;

    public bool IsLocal => LobbyPlayer.IsLocal();

    public bool IsMaster => LobbyPlayer.GetHoloNetPlayer().IsMasterClient();

    public void SetMasterClient()
    {
        LobbyPlayer.GetHoloNetPlayer().SetMasterClient();
    }

    public void NotifyMasterChanged()
    {
        OnPropertyChanged(nameof(IsMaster));
    }
}
