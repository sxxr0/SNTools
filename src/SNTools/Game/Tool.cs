using CommunityToolkit.Mvvm.ComponentModel;

namespace SNTools.Game;

public abstract class Tool : ObservableObject
{
    public string Id { get; }
    public GameMode[] SupportedGameModes { get; }

    protected Tool(params GameMode[] supportedGameModes)
    {
        Id = GetType().FullName!;
        SupportedGameModes = supportedGameModes;
    }

    public static T Get<T>() where T : Tool, new()
        => Singleton<T>.Instance;

    private class Singleton<T> where T : Tool, new()
    {
        public static T Instance { get; } = new();
    }
}