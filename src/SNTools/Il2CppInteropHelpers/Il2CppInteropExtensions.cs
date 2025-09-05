using System.Collections;

namespace SNTools.Il2CppInteropHelpers;

internal static class Il2CppInteropExtensions
{
    public static IEnumerable<T> Wrap<T>(this Il2CppSystem.Collections.Generic.IEnumerable<T> enumerable)
        => new Il2CppEnumerableWrapper<T>(enumerable);

    public static IEnumerable Wrap(this Il2CppSystem.Collections.IEnumerable enumerable)
        => new Il2CppEnumerableWrapper(enumerable);

    public static IEnumerable<T> Wrap<T>(this Il2CppSystem.Collections.Generic.List<T> enumerable)
        => enumerable.Cast<Il2CppSystem.Collections.Generic.IEnumerable<T>>().Wrap();
}
