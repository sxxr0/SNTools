using GameModes.Shared.Models.Customization;

namespace SNTools.Game.Tools.Skins;

public class SkinsUnlocker() : ToggleTool(GameMode.MENU, GameMode.LOBBY)
{
    private static (CustomizationItemInfo info, bool isDefault)[]? _allItems;

    protected override void Disable()
    {
        foreach (var (info, isDefault) in _allItems!)
        {
            info.isDefaultValue = isDefault;
        }
    }

    protected override void Enable()
    {
        InitializeItemLists();

        foreach (var (info, _) in _allItems!)
        {
            info.isDefaultValue = true;
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