using GameModes.Shared.Models.Customization;
using HarmonyLib;

namespace SNTools.Game.Tools.Skins;

[HarmonyPatch]
internal static class SkinsUnlocker
{
    private static (CustomizationItemInfo info, bool isDefault)[]? _allItems;

    private static bool _enabled;

    public static bool Enabled
    {
        get => _enabled;
        set
        {
            if (value == _enabled)
                return;

            _enabled = value;

            InitializeItemLists();

            foreach (var (info, isDefault) in _allItems!)
            {
                info.isDefaultValue = value || isDefault;
            }
        }
    }

    private static void InitializeItemLists()
    {
        if (_allItems == null)
        {
            var items = UnityEngine.Resources.FindObjectsOfTypeAll<CustomizationItemInfo>();
            _allItems = new (CustomizationItemInfo, bool)[items.Length];
            for (var idx = 0; idx < _allItems.Length; idx++)
            {
                var item = items[idx];
                _allItems[idx] = new(item, item.isDefaultValue);
            }
        }
    }
}