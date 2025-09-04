using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.UI.Models;
using SNTools.UI.Pages.ViewModels;
using System.Collections.ObjectModel;

namespace SNTools.UI.Windows.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private Category? _currentCategory;

    public ReadOnlyCollection<Category> Categories { get; } = new List<Category>()
    {
        new("Login", new LoginPageViewModel(), GameMode.PRELOAD),
        new("Identity", new IdentityPageViewModel(), GameMode.MENU),
        new("Lobby Players", new LobbyPlayersPageViewModel(), GameMode.LOBBY),
        new("Skins", new SkinsPageViewModel(), GameMode.MENU, GameMode.LOBBY),
        new("Lobbies", new LobbiesPageViewModel(), GameMode.MENU)
    }.AsReadOnly();

    public ObservableCollection<Category> ActiveCategories { get; } = [];

    public string AppVersion => $"v{Program.Version}";

    public MainWindowViewModel()
    {
        GameModController.GameModeChanged += OnGameModeChanged;
        OnGameModeChanged(GameModController.CurrentGameMode); // ensure initial population
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        var lastCategory = CurrentCategory;

        ActiveCategories.Clear();
        foreach (var category in Categories)
        {
            if (category.GameMode.Contains(gameMode))
            {
                ActiveCategories.Add(category);
            }
        }

        CurrentCategory = lastCategory != null && lastCategory.GameMode.Contains(gameMode) ? lastCategory : ActiveCategories.FirstOrDefault();
    }

    public void Dispose()
    {
        GameModController.GameModeChanged -= OnGameModeChanged;
        GC.SuppressFinalize(this);
    }
}