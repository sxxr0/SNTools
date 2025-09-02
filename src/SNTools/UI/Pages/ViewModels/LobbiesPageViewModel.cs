using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SNTools.Game;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace SNTools.UI.Pages.ViewModels;

public partial class LobbiesPageViewModel : ObservableObject
{
    private readonly DispatcherTimer _refreshTimer = new()
    {
        Interval = TimeSpan.FromSeconds(1)
    };

    public ObservableCollection<RoomWrapper> Lobbies => RoomManager.Rooms;

    public LobbiesPageViewModel()
    {
        _refreshTimer.Tick += RefreshTick;
    }

    private void RefreshTick(object? sender, EventArgs e)
    {
        RoomManager.RefreshList();
    }

    internal void Load()
    {
        RoomManager.RefreshList();
        _refreshTimer.Start();
    }

    internal void Unload()
    {
        _refreshTimer.Stop();
    }

    [RelayCommand]
    private void JoinLobby(RoomWrapper room)
    {
        room.Join();
    }
}