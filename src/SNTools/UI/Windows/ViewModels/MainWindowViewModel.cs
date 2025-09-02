using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.UI.Models;
using SNTools.UI.Pages;
using System.Collections.ObjectModel;

namespace SNTools.UI.Windows.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private Category? _currentCategory;

    public ReadOnlyCollection<Category> Categories { get; } = new List<Category>()
    {
        new("Identity", new IdentityPage(), GameMode.MENU),
        new("Skins", new SkinsPage(), GameMode.MENU, GameMode.LOBBY)
    }.AsReadOnly();

    public ObservableCollection<Category> ActiveCategories { get; } = [];

    public MainWindowViewModel()
    {
        GameModController.GameModeChanged += OnGameModeChanged;
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