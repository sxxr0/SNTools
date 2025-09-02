using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace SNTools.UI.Models;

public class Category(string name, UserControl content, params GameMode[] gameMode) : ObservableObject
{
    public string Name { get; } = name;
    public UserControl Content { get; } = content;
    public GameMode[] GameMode { get; } = gameMode;
}