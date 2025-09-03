using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using System.Windows.Media;

namespace SNTools.UI.Models;

public partial class LobbyPlayerButtonModel(int slot, Brush color) : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Enabled))]
    [NotifyPropertyChangedFor(nameof(DisplayName))]
    [NotifyPropertyChangedFor(nameof(Id))]
    private LobbyPlayerWrapper? _lobbyPlayer;

    public Brush Color { get; } = color;

    public int Slot { get; } = slot;

    public bool Enabled => LobbyPlayer != null;

    public string DisplayName => LobbyPlayer?.DisplayName ?? "Empty slot";

    public string? Id => LobbyPlayer?.UnreliableId;

    public bool IsLocal => LobbyPlayer?.IsLocal ?? false;
}