namespace SNTools.Game;

public abstract class TriggerTool : Tool
{
    private bool _blocked;

    protected TriggerTool(params GameMode[] supportedGameModes) : base(supportedGameModes)
    {
        _blocked = !SupportedGameModes.Contains(GameModController.CurrentGameMode);

        GameModController.GameModeChanged += OnGameModeChanged;
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        _blocked = !SupportedGameModes.Contains(gameMode);
    }

    public void Trigger()
    {
        if (_blocked)
            return;

        Execute();
    }

    protected abstract void Execute();
}
