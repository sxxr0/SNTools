using CommunityToolkit.Mvvm.ComponentModel;

namespace SNTools.Game;

public partial class LobbyPlayerWrapper(LobbyPlayer lobbyPlayer, PlayerInfo playerInfo) : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayName))]
    [NotifyPropertyChangedFor(nameof(UnreliableId))]
    [NotifyPropertyChangedFor(nameof(HasFakeId))]
    [NotifyPropertyChangedFor(nameof(HasFakeName))]
    private PlayerInfo _playerInfo = playerInfo;

    public LobbyPlayer LobbyPlayer { get; } = lobbyPlayer;

    public string DisplayName => PlayerInfo.GetName();

    public string NickName
#if STEAM
        => LobbyPlayer.GetHoloNetPlayer().GetPhotonPlayer().GetNickName();
#elif GDK
        => DisplayName;
#endif

    public string UnreliableId => PlayerInfo.GetID();

    public string Id
#if STEAM
        => LobbyPlayer.GetHoloNetPlayer().GetPhotonPlayer().GetId();
#elif GDK
        => UnreliableId;
#endif

    public int LobbySlot => LobbyPlayer.field_Public_LobbyCharacter_0?._lobbyPosition ?? -1;

    public bool IsLocal => LobbyPlayer.IsLocal();

    public bool IsMaster => LobbyPlayer.GetHoloNetPlayer().IsMasterClient();

    public bool HasFakeId => UnreliableId != Id;

    public bool HasFakeName => DisplayName != NickName;

    public void SetMasterClient()
    {
        LobbyPlayer.GetHoloNetPlayer().SetMasterClient();
    }

    public void NotifyMasterChanged()
    {
        OnPropertyChanged(nameof(IsMaster));
    }

    public void NotifyNameChanged()
    {
        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(HasFakeName));
    }
}
