using SNTools.UI.Pages.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SNTools.UI.Pages;

/// <summary>
/// Interaction logic for LobbiesPage.xaml
/// </summary>
public partial class LobbiesPage : UserControl
{
    public LobbiesPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is LobbiesPageViewModel vm)
            vm.Unload();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is LobbiesPageViewModel vm)
            vm.Load();
    }
}
