using System.ComponentModel;
using System.Windows.Controls;

namespace SNTools.UI.Models;

public class Category(string name, UserControl content, GameMode gameMode) : INotifyPropertyChanged
{
    public string Name { get; } = name;
    public UserControl Content { get; } = content;
    public GameMode GameMode { get; } = gameMode;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}