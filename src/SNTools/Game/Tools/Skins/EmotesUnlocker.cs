using GameModes.Shared.Models.Customization;

namespace SNTools.Game.Tools.Skins;

public class EmotesUnlocker() : ToggleTool(GameMode.MENU, GameMode.LOBBY)
{
    private static (CustomizationEmotionsInfo info, ActorType actorType, bool isDefault)[]? _allEmotions;

    protected override void Disable()
    {
        foreach (var (info, actorType, isDefault) in _allEmotions!)
        {
            info.allowedActorType = actorType;
            info.isDefaultValue = isDefault;
        }
    }

    protected override void Enable()
    {
        InitializeItemLists();

        foreach (var (info, _, _) in _allEmotions!)
        {
            info.allowedActorType = ActorType.ANY;
            info.isDefaultValue = true;
        }
    }

    private static void InitializeItemLists()
    {
        if (_allEmotions == null)
        {
            var emotions = UnityEngine.Resources.FindObjectsOfTypeAll<CustomizationEmotionsInfo>();
            _allEmotions = new (CustomizationEmotionsInfo, ActorType, bool)[emotions.Length];
            for (var idx = 0; idx < _allEmotions.Length; idx++)
            {
                var emotion = emotions[idx];
                _allEmotions[idx] = new(emotion, emotion.allowedActorType, emotion.isDefaultValue);
            }
        }
    }
}