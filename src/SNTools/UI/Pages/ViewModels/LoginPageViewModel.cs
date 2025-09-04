using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.Game.Tools.Identity;

namespace SNTools.UI.Pages.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    public IdSpoofer IdSpoofer { get; } = Tool.Get<IdSpoofer>();
}