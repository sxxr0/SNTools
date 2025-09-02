using CommunityToolkit.Mvvm.ComponentModel;

namespace SNTools.Game;

public abstract partial class ToggleTool : Tool
{
    private bool _blocked;

    [ObservableProperty]
    private bool _enabled;

    protected ToggleTool(params GameMode[] supportedGameModes) : base(supportedGameModes)
    {
        _blocked = !SupportedGameModes.Contains(GameModController.CurrentGameMode);

        GameModController.GameModeChanged += OnGameModeChanged;
        GameModController.GameModeChanging += OnGameModeChanging;
    }

    private void OnGameModeChanging(GameMode gameMode)
    {
        if (!SupportedGameModes.Contains(gameMode) && !_blocked && Enabled)
            Disable();
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        var supported = SupportedGameModes.Contains(gameMode);
        if (supported && _blocked && Enabled)
            Enable();

        _blocked = !supported;
    }

    partial void OnEnabledChanged(bool value)
    {
        if (_blocked)
            return;

        if (value)
            Enable();
        else
            Disable();
    }

    protected abstract void Enable();

    protected abstract void Disable();
}