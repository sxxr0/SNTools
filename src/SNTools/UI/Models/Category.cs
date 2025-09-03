using CommunityToolkit.Mvvm.ComponentModel;

namespace SNTools.UI.Models;

public class Category(string name, ObservableObject viewModel, params GameMode[] gameMode) : ObservableObject
{
    public string Name { get; } = name;
    public ObservableObject ViewModel { get; } = viewModel;
    public GameMode[] GameMode { get; } = gameMode;
}