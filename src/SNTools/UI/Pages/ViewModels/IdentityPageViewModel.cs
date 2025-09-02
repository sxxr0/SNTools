using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game.Features.Identity;

namespace SNTools.UI.Pages.ViewModels;

public partial class IdentityPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _unlockSkins;

    [ObservableProperty]
    private bool _unlockEmotes;

    partial void OnUnlockSkinsChanged(bool value)
    {
        SkinsUnlocker.Enabled = value;
    }

    partial void OnUnlockEmotesChanged(bool value)
    {
        EmotesUnlocker.Enabled = value;
    }
}