using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SNTools.UI.Controls;

/// <summary>
/// Interaction logic for SubWindow.xaml
/// </summary>
public partial class SubWindow : UserControl
{
    private bool _isDragging;
    private Point _clickPosition;

    public SubWindow()
    {
        InitializeComponent();
    }

    private void HeaderMouseLeftDown(object sender, MouseButtonEventArgs args)
    {
        _isDragging = true;
        _clickPosition = args.GetPosition(this);
        Mouse.Capture((UIElement)sender);
    }

    private void HeaderMouseLeftUp(object sender, MouseButtonEventArgs args)
    {
        _isDragging = false;
        Mouse.Capture(null);
    }

    private void HeaderMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging)
            return;

        var header = (Border)sender;
        var parent = (Canvas)Parent;

        var mousePos = e.GetPosition(parent);

        mousePos.X -= _clickPosition.X;
        mousePos.Y -= _clickPosition.Y;
        mousePos.X = Math.Clamp(mousePos.X, 0, parent.ActualWidth - header.ActualWidth);
        mousePos.Y = Math.Clamp(mousePos.Y, 0, parent.ActualHeight - header.ActualHeight);

        Canvas.SetLeft(this, mousePos.X);
        Canvas.SetTop(this, mousePos.Y);
    }
}
