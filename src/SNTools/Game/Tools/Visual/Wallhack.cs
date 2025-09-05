using GameModes.GameplayMode.Cameras;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace SNTools.Game.Tools.Visual;

public class Wallhack() : ToggleTool(GameMode.GAMEPLAY)
{
    private readonly List<Transform> _rootBones = [];

    protected override void Disable()
    {
        ToolsDrawing.RemoveDrawingSource(EnumerateLines);

        _rootBones.Clear();
    }

    protected override void Enable()
    {
        var players = PlayerControllerAPI.Players;
        if (players == null)
        {
            Enabled = false;
            return;
        }

        foreach (var player in players)
        {
            var actors = player.GetActors();
            foreach (var actor in actors)
            {
                var rootBone = actor.transform.Find("ThirdPerson/root/en_root");
                if (rootBone == null)
                    continue;

                rootBone.Find("spine1/spine2/Flashlight")?.gameObject.SetActive(false);
                rootBone.Find("spine1/spine2/bn_chest/r_clavicle/r_shoulder/patch_game Variant")?.gameObject.SetActive(false);
                rootBone.Find("spine1/spine2/bn_chest/Highlightable")?.gameObject.SetActive(false);
                rootBone.Find("spine1/HoldChildrenPoint")?.gameObject.SetActive(false);
                rootBone.Find("spine1/spine2/bn_chest/bag_start")?.gameObject.SetActive(false);
                rootBone.Find("spine1/spine2/bn_chest/bag_top")?.gameObject.SetActive(false);

                DisableChildren(rootBone.Find("spine1/spine2/bn_chest/neck/head"));
                //DisableChildren(rootBone.Find("spine1/spine2/bn_chest/l_clavicle/l_shoulder/l_elbow/l_hand"));
                //DisableChildren(rootBone.Find("spine1/spine2/bn_chest/r_clavicle/r_shoulder/r_elbow/r_hand"));

                _rootBones.Add(rootBone);
            }
        }

        ToolsDrawing.AddDrawingSource(EnumerateLines);
    }

    private static void DisableChildren(Transform? t)
    {
        if (t == null)
            return;

        foreach (object childObj in t)
        {
            var child = ((Il2CppObjectBase)childObj).Cast<Transform>();
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerable<ToolsDrawing.Line> EnumerateLines()
    {
        var cam = GameCamera.instance?.prop_Camera_0;
        if (cam == null)
            yield break;

        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        IEnumerable<ToolsDrawing.Line> EnumerateChildren(Transform t, Vector3 screenPos)
        {
            foreach (object childObj in t)
            {
                var child = ((Il2CppObjectBase)childObj).Cast<Transform>();

                if (!child.gameObject.activeInHierarchy)
                    continue;

                var childScreen = cam.WorldToScreenPoint(child.position);

                // Checks if the line is at all visible on screen
                if ((screenPos.z > 1 || childScreen.z > 1)
                    && ((screenPos.x > 0 && screenPos.x < screenWidth) || (childScreen.x > 0 && childScreen.x < screenWidth))
                    && ((screenPos.y > 0 && screenPos.y < screenHeight) || (childScreen.y > 0 && childScreen.y < screenHeight)))
                {
                    yield return new ToolsDrawing.Line
                    {
                        X1 = (double)screenPos.x / screenWidth,
                        Y1 = 1d - ((double)screenPos.y / screenHeight),
                        X2 = (double)childScreen.x / screenWidth,
                        Y2 = 1d - ((double)childScreen.y / screenHeight),
                        Color = new(0, 255, 0)
                    };
                }

                if (child.childCount != 0)
                    foreach (var line in EnumerateChildren(child, childScreen))
                        yield return line;
            }
        }

        foreach (var rootBone in _rootBones)
        {
            if (rootBone == null || !rootBone.gameObject.activeInHierarchy)
                continue;

            foreach (var line in EnumerateChildren(rootBone, cam.WorldToScreenPoint(rootBone.position)))
                yield return line;
        }
    }
}
