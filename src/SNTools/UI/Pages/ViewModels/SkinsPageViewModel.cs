using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game.Features.Skins;

namespace SNTools.UI.Pages.ViewModels;

public partial class SkinsPageViewModel : ObservableObject
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