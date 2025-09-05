using SNTools.Game;
using SNTools.UI.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SNTools.UI.Controls;

internal class ToolsCanvas : FrameworkElement
{
    // Cache frozen Pens by ARGB to avoid per-frame allocations
    private static readonly Dictionary<uint, Pen> s_penCache = [];

    private ToolsCanvasViewModel ViewModel => (ToolsCanvasViewModel)DataContext;

    public ToolsCanvas()
    {
        DataContext = new ToolsCanvasViewModel();

        RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        DataContextChanged += OnDataContextChanged;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var lines = ViewModel.Lines;
        if (lines == null)
            return;

        foreach (var line in lines)
        {
            var pen = GetPen(line.Color);
            drawingContext.DrawLine(pen, LinePosToPoint(line.X1, line.Y1), LinePosToPoint(line.X2, line.Y2));
        }
    }

    private Point LinePosToPoint(double x, double y)
        => new(x * ActualWidth, y * ActualHeight);

    private static Pen GetPen(ToolsDrawing.Color color)
    {
        // Treat fully default (0) as opaque white for visibility
        var argb = color.ToArgb32();
        if (argb == 0)
            argb = 0xFFFFFFFF;

        if (s_penCache.TryGetValue(argb, out var cached))
            return cached;

        var brush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        brush.Freeze();
        var pen = new Pen(brush, 2.0);
        pen.Freeze();

        s_penCache[argb] = pen;
        return pen;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ToolsCanvasViewModel.Lines))
            InvalidateVisual();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        ViewModel.Load();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        ViewModel.Unload();
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is ToolsCanvasViewModel oldVm)
            oldVm.PropertyChanged -= OnViewModelPropertyChanged;

        if (e.NewValue is ToolsCanvasViewModel newVm)
            newVm.PropertyChanged += OnViewModelPropertyChanged;
    }
}
