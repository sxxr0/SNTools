namespace SNTools.Game;

public static class ToolsDrawing
{
    private static bool _active;
    private static readonly HashSet<Func<IEnumerable<Line>>> _drawingSources = [];

    public static event Action<IEnumerable<Line>?>? DrawLines;

    public static void AddDrawingSource(Func<IEnumerable<Line>> source)
    {
        if (!_active)
        {
            GameModController.Update += OnUpdate;
            _active = true;
        }

        _drawingSources.Add(source);
    }

    public static void RemoveDrawingSource(Func<IEnumerable<Line>> source)
    {
        _drawingSources.Remove(source);

        if (_active && _drawingSources.Count == 0)
        {
            GameModController.Update -= OnUpdate;

            // Trigger redraw to clear existing lines
            DrawLines?.Invoke(null);

            _active = false;
        }
    }

    private static void OnUpdate()
    {
        if (!_active)
            return;

        DrawLines?.Invoke(GetLinesToDraw());
    }

    private static IEnumerable<Line> GetLinesToDraw()
    {
        foreach (var source in _drawingSources)
        {
            var lines = source();
            foreach (var line in lines)
                yield return line;
        }
    }

    public struct Color
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public readonly uint ToArgb32()
            => ((uint)A << 24) | ((uint)R << 16) | ((uint)G << 8) | B;
    }

    public struct Line
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public Color Color { get; set; }
    }
}
