using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.Game.Tools.Skins;

namespace SNTools.UI.Pages.ViewModels;

public partial class SkinsPageViewModel : ObservableObject
{
    public SkinsUnlocker SkinsUnlocker { get; } = Tool.Get<SkinsUnlocker>();

    public EmotesUnlocker EmotesUnlocker { get; } = Tool.Get<EmotesUnlocker>();
}