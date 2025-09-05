using System.Collections;

namespace SNTools.Il2CppInteropHelpers;

public class Il2CppEnumeratorWrapper(Il2CppSystem.Collections.IEnumerator inner) : IEnumerator
{
    public object Current => inner.Current;

    public bool MoveNext()
        => inner.MoveNext();

    public void Reset()
        => inner.Reset();
}

public class Il2CppEnumeratorWrapper<T>(Il2CppSystem.Collections.Generic.IEnumerator<T> inner) : Il2CppEnumeratorWrapper(inner.Cast<Il2CppSystem.Collections.IEnumerator>()), IEnumerator<T>
{
    public new T Current => inner.Current;

    public void Dispose()
    {
        inner.Cast<Il2CppSystem.IDisposable>().Dispose();
        GC.SuppressFinalize(this);
    }
}