using SNTools.UI.Pages.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SNTools.UI.Pages;

/// <summary>
/// Interaction logic for LobbyPlayersPage.xaml
/// </summary>
public partial class LobbyPlayersPage : UserControl
{
    public LobbyPlayersPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is LobbyPlayersPageViewModel vm)
            vm.Unload();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is LobbyPlayersPageViewModel vm)
            vm.Load();
    }
}
