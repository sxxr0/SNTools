using HarmonyLib;
using UnityEngine;

namespace SNTools.Game;

[HarmonyPatch]
internal static class UnityCursorOverride
{
    private static bool _alwaysVisible;
    private static bool _visible;
    private static CursorLockMode _lockState;

    public static bool AlwaysVisible
    {
        get => _alwaysVisible;
        set
        {
            if (_alwaysVisible == value)
                return;

            if (value)
            {
                _visible = Cursor.visible;
                _lockState = Cursor.lockState;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                _alwaysVisible = true;
            }
            else
            {
                _alwaysVisible = false;

                Cursor.visible = _visible;
                Cursor.lockState = _lockState;
            }
        }
    }

    [HarmonyPatch(typeof(Cursor), "visible", MethodType.Setter)]
    [HarmonyPrefix]
    private static bool CursorSetVisible([HarmonyArgument(0)] bool value)
    {
        if (!_alwaysVisible)
            return true;

        _visible = value;
        return false;
    }

    [HarmonyPatch(typeof(Cursor), "visible", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool CursorGetVisible(ref bool __result)
    {
        if (!_alwaysVisible)
            return true;

        __result = _visible;
        return false;
    }

    [HarmonyPatch(typeof(Cursor), "lockState", MethodType.Setter)]
    [HarmonyPrefix]
    private static bool CursorSetLockState([HarmonyArgument(0)] CursorLockMode value)
    {
        if (!_alwaysVisible)
            return true;

        _lockState = value;
        return false;
    }

    [HarmonyPatch(typeof(Cursor), "lockState", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool CursorGetLockState(ref CursorLockMode __result)
    {
        if (!_alwaysVisible)
            return true;

        __result = _lockState;
        return false;
    }
}
