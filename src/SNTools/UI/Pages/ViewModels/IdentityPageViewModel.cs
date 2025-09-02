using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.Game.Tools.Identity;

namespace SNTools.UI.Pages.ViewModels;

public partial class IdentityPageViewModel : ObservableObject
{
    public NameChanger NameChanger { get; } = Tool.Get<NameChanger>();
}