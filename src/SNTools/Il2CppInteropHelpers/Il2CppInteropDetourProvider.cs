using Il2CppInterop.Runtime.Injection;
using System.Runtime.InteropServices;

namespace SNTools.Il2CppInteropHelpers;

internal sealed class Il2CppInteropDetourProvider : IDetourProvider
{
    public IDetour Create<TDelegate>(nint original, TDelegate target) where TDelegate : Delegate
    {
        return new Il2CppInteropDetour(original, target);
    }

    private sealed class Il2CppInteropDetour : IDetour
    {
        private readonly Delegate _target;

        public nint Target { get; }
        public nint Detour { get; private set; }
        public nint OriginalTrampoline { get; private set; }

        public Il2CppInteropDetour(nint detourFrom, Delegate target)
        {
            Target = detourFrom;
            _target = target;

            Apply();
        }

        public unsafe void Apply()
        {
            if (Detour != 0)
                return;

            Detour = Marshal.GetFunctionPointerForDelegate(_target);

            OriginalTrampoline = Dobby.HookAttach(Target, Detour);
        }

        public unsafe void Dispose()
        {
            if (Detour == 0)
                return;

            Dobby.HookDetach(Target);

            Detour = 0;
            OriginalTrampoline = 0;
        }

        public T GenerateTrampoline<T>() where T : Delegate
        {
            return OriginalTrampoline == 0 ? null! : Marshal.GetDelegateForFunctionPointer<T>(OriginalTrampoline);
        }
    }
}