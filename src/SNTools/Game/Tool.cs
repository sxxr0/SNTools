using CommunityToolkit.Mvvm.ComponentModel;
using Tomlet.Models;

namespace SNTools.Game;

public abstract class Tool : ObservableObject
{
    public string Id { get; }

    public GameMode[] SupportedGameModes { get; }

    protected Tool(params GameMode[] supportedGameModes)
    {
        Id = GetType().Name;
        SupportedGameModes = supportedGameModes;
    }

    public static T Get<T>() where T : Tool, new()
        => Singleton<T>.Instance;

    private static class Singleton<T> where T : Tool, new()
    {
        public static T Instance { get; }

        static Singleton()
        {
            Instance = new();
            Instance.LoadSetting(ToolsConfig.Document);
            ToolsConfig.Saving += () => Instance.SaveSetting(ToolsConfig.Document);
        }
    }

    public virtual void SaveSetting(TomlDocument document) { }

    public virtual void LoadSetting(TomlDocument document) { }
}