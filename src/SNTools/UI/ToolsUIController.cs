using Microsoft.Extensions.Logging;
using SNTools.Win32;
using System.Diagnostics;
using System.Windows;

namespace SNTools.UI;

internal static class ToolsUIController
{
    private static RECT _lastGameWindowRect;
    private static nint _gameWindowHandle;

    private static OverlayWindow? _overlayWindow;

    /// <remarks>
    /// Has to be ran AFTER the game has initialized its own window
    /// </remarks>
    public static void InitGameOverlay()
    {
        if (_overlayWindow != null)
            return;

        _gameWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
        if (_gameWindowHandle == 0)
            throw new InvalidOperationException("Cannot initialize overlay before the game window");

        _ = new App();
        _overlayWindow = new OverlayWindow();
        _overlayWindow.Show();

        UpdateWindowPosition();
    }

    public static void Update()
    {
        UpdateWindowPosition();
    }

    private static void UpdateWindowPosition()
    {
        if (_overlayWindow == null)
            return;

        var shouldShow = _overlayWindow.IsActive || UnityEngine.Application.isFocused;
        if (_overlayWindow.IsVisible != shouldShow)
            _overlayWindow.Visibility = shouldShow ? Visibility.Visible : Visibility.Hidden;

        if (!shouldShow || !PInvoke.GetClientRect(_gameWindowHandle, out var rectRaw))
            return;

        var topLeftRaw = new POINT() { X = rectRaw.Left, Y = rectRaw.Top };
        PInvoke.ClientToScreen(_gameWindowHandle, ref topLeftRaw);

        var bottomRightRaw = new POINT() { X = rectRaw.Right, Y = rectRaw.Bottom };
        PInvoke.ClientToScreen(_gameWindowHandle, ref bottomRightRaw);

        var rect = new RECT()
        {
            Left = topLeftRaw.X,
            Top = topLeftRaw.Y,
            Right = bottomRightRaw.X,
            Bottom = bottomRightRaw.Y
        };

        if (_lastGameWindowRect == rect)
            return;

        _lastGameWindowRect = rect;

        Program.MainLogger.LogInformation("Updating window pos");

        var hwndSource = PresentationSource.FromVisual(_overlayWindow);
        if (hwndSource.CompositionTarget == null)
            return;

        var m = hwndSource.CompositionTarget.TransformFromDevice;
        var topLeft = m.Transform(new Point(topLeftRaw.X, topLeftRaw.Y));
        var bottomRight = m.Transform(new Point(bottomRightRaw.X, bottomRightRaw.Y));

        _overlayWindow.Left = topLeft.X;
        _overlayWindow.Top = topLeft.Y;
        _overlayWindow.Width = bottomRight.X - topLeft.X;
        _overlayWindow.Height = bottomRight.Y - topLeft.Y;
    }
}
