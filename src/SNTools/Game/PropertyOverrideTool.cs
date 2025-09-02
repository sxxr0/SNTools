using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SNTools.Game;

public abstract partial class PropertyOverrideTool<T> : Tool
{
    private bool _blocked;

    [ObservableProperty]
    private T? _value;

    protected PropertyOverrideTool(params GameMode[] supportedGameModes) : base(supportedGameModes)
    {
        _blocked = !SupportedGameModes.Contains(GameModController.CurrentGameMode);

        GameModController.GameModeChanged += OnGameModeChanged;
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        var supported = SupportedGameModes.Contains(gameMode);

        if (supported && _blocked && !IsDefault())
            SetValue(Value);

        _blocked = !supported;
    }

    partial void OnValueChanged(T? value)
    {
        if (_blocked)
            return;

        if (IsDefault())
            RestoreDefault();
        else
            SetValue(Value);
    }

    [MemberNotNullWhen(false, nameof(Value))]
    protected virtual bool IsDefault()
        => Value == null;

    protected abstract void RestoreDefault();
    protected abstract void SetValue(T value);
}
