using GameModes.Shared.Models.Customization;

namespace SNTools.Game.Features.Skins;

internal static class EmotesUnlocker
{
    private static (CustomizationEmotionsInfo info, ActorType actorType, bool isDefault)[]? _allEmotions;

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
            foreach (var (info, _, isDefault) in _allEmotions!)
            {
                info.allowedActorType = value ? ActorType.ANY : info.allowedActorType;
                info.isDefaultValue = value || isDefault;
            }
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