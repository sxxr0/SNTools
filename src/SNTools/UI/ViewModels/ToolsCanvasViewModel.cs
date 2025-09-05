using CommunityToolkit.Mvvm.ComponentModel;
using SNTools.Game;

namespace SNTools.UI.ViewModels;

internal partial class ToolsCanvasViewModel : ObservableObject
{
    private readonly bool _skipFrame;

    [ObservableProperty]
    private IEnumerable<ToolsDrawing.Line>? _lines;

    private void OnDrawLines(IEnumerable<ToolsDrawing.Line>? obj)
    {
        Lines = obj;
    }

    internal void Load()
    {
        ToolsDrawing.DrawLines += OnDrawLines;
    }

    internal void Unload()
    {
        ToolsDrawing.DrawLines -= OnDrawLines;
    }
}
