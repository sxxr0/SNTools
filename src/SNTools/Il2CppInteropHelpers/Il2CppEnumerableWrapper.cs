using System.Collections;

namespace SNTools.Il2CppInteropHelpers;

public class Il2CppEnumerableWrapper(Il2CppSystem.Collections.IEnumerable source) : IEnumerable
{
    protected virtual Il2CppEnumeratorWrapper CreateWrapper()
        => new(source.GetEnumerator());

    public IEnumerator GetEnumerator()
        => CreateWrapper();
}

public class Il2CppEnumerableWrapper<T>(Il2CppSystem.Collections.Generic.IEnumerable<T> source) : Il2CppEnumerableWrapper(source.Cast<Il2CppSystem.Collections.IEnumerable>()), IEnumerable<T>
{
    protected override Il2CppEnumeratorWrapper CreateWrapper()
        => new Il2CppEnumeratorWrapper<T>(source.GetEnumerator());

    public new IEnumerator<T> GetEnumerator()
        => (Il2CppEnumeratorWrapper<T>)CreateWrapper();
}