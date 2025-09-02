using SNTools.UI.Pages.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SNTools.UI.Pages;

/// <summary>
/// Interaction logic for IdentityPage.xaml
/// </summary>
public partial class IdentityPage : UserControl
{
    public IdentityPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is IdentityPageViewModel vm)
        {
            vm.Refresh();
        }
    }
}
