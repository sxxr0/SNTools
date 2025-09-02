using HarmonyLib;

namespace SNTools;

internal static class HarmonyExtensions
{
    public static int PatchObfuscated(this Harmony harmony, Type type, string methodName, HarmonyMethod? prefix = null, HarmonyMethod? postfix = null)
    {
        if (methodName == null || methodName.Length < 3)
            return 0;

        var lastChar = methodName[^1];
        if (lastChar < '0' || lastChar > '9' || methodName[^2] != '_')
            return 0;

        methodName = methodName[..^1];

        if (methodName.EndsWith("PDM_"))
            methodName = methodName[..^4];

        var count = 0;
        var methods = type.GetMethods();
        foreach (var m in methods)
        {
            if (!m.Name.StartsWith(methodName))
                continue;

            harmony.Patch(m, prefix, postfix);

            count++;
        }

        return count;
    }
}