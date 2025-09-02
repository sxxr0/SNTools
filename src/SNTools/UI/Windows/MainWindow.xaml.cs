using System.Windows.Controls;

namespace SNTools.UI.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UserControl
{
    public MainWindow()
    {
        InitializeComponent();
        Unloaded += MainWindow_Unloaded;
    }

    private void MainWindow_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
