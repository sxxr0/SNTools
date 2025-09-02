using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SNTools.UI.Controls;

public class SubWindow : ContentControl
{
    private bool _isDragging;
    private Point _clickPosition;

    static SubWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SubWindow), new FrameworkPropertyMetadata(typeof(SubWindow)));
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(SubWindow), new PropertyMetadata(string.Empty));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var header = (Border)GetTemplateChild("Header");
        header.MouseLeftButtonDown += HeaderMouseLeftDown;
        header.MouseLeftButtonUp += HeaderMouseLeftUp;
        header.MouseMove += HeaderMouseMove;
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

        var header = (FrameworkElement)sender;
        var canvas = FindCanvas(this);
        if (canvas == null)
            return;

        var canvasChild = FindCanvasChild(this);
        if (canvasChild == null)
            return;

        var mousePos = e.GetPosition(canvas);

        mousePos.X -= _clickPosition.X;
        mousePos.Y -= _clickPosition.Y;
        mousePos.X = Math.Clamp(mousePos.X, 0, canvas.ActualWidth - header.ActualWidth);
        mousePos.Y = Math.Clamp(mousePos.Y, 0, canvas.ActualHeight - header.ActualHeight);

        Canvas.SetLeft(canvasChild, mousePos.X);
        Canvas.SetTop(canvasChild, mousePos.Y);
    }

    private static Canvas? FindCanvas(FrameworkElement element)
        => element is Canvas c ? c : (element.Parent is FrameworkElement f ? FindCanvas(f) : null);

    private static FrameworkElement? FindCanvasChild(FrameworkElement element)
        => element.Parent is Canvas ? element : (element.Parent is FrameworkElement f ? FindCanvasChild(f) : null);
}
