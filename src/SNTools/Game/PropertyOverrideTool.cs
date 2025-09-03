using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Tomlet;
using Tomlet.Models;

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

    public override void SaveSetting(TomlDocument document)
    {
        if (IsDefault())
            document.Entries.Remove(Id);
        else
            document.Entries[Id] = TomletMain.ValueFrom(Value);
    }

    public override void LoadSetting(TomlDocument document)
    {
        if (document.TryGetValue(Id, out var value))
        {
            try
            {
                Value = TomletMain.To<T>(value);
            }
            catch { }
        }
    }

    [MemberNotNullWhen(false, nameof(Value))]
    protected virtual bool IsDefault()
        => Value == null;

    protected abstract void RestoreDefault();
    protected abstract void SetValue(T value);
}
