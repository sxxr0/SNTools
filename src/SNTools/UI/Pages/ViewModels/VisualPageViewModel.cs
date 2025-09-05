using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.Game.Tools.Visual;

namespace SNTools.UI.Pages.ViewModels;

public partial class VisualPageViewModel : ObservableObject
{
    public Wallhack Wallhack { get; } = Tool.Get<Wallhack>();
}
