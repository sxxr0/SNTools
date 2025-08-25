using System.Runtime.InteropServices;

namespace SNTools.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct RECT : IEquatable<RECT>
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public override readonly bool Equals(object? obj) =>
        obj is RECT other && Equals(other);

    public override readonly int GetHashCode() =>
        HashCode.Combine(Left, Top, Right, Bottom);

    public readonly bool Equals(RECT other) =>
        Left == other.Left &&
        Top == other.Top &&
        Right == other.Right &&
        Bottom == other.Bottom;

    public static bool operator ==(RECT left, RECT right) => left.Equals(right);
    public static bool operator !=(RECT left, RECT right) => !left.Equals(right);
}