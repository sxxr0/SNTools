using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;
using SNTools.UI.Models;
using SNTools.UI.Pages;
using System.Collections.ObjectModel;

namespace SNTools.UI.Windows.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    public ReadOnlyCollection<Category> Categories { get; } = new List<Category>()
    {
        new("Identity", new IdentityPage(), GameMode.MENU)
    }.AsReadOnly();

    public ObservableCollection<Category> ActiveCategories { get; } = [];

    public MainWindowViewModel()
    {
        GameModController.GameModeChanged += OnGameModeChanged;
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        ActiveCategories.Clear();
        foreach (var category in Categories)
        {
            if (category.GameMode == gameMode)
            {
                ActiveCategories.Add(category);
            }
        }
    }

    public void Dispose()
    {
        GameModController.GameModeChanged -= OnGameModeChanged;
        GC.SuppressFinalize(this);
    }
}