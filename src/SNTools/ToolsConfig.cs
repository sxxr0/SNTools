using Microsoft.Extensions.Logging;
using System.IO;
using Tomlet;
using Tomlet.Models;

namespace SNTools;

internal static class ToolsConfig
{
    public static string ConfigPath { get; } = Path.Combine(Program.AppDataDir, "config.cfg");
    public static TomlDocument Document { get; }

    public static event Action? Saving;

    static ToolsConfig()
    {
        if (File.Exists(ConfigPath))
        {
            Program.MainLogger.LogInformation("Loading config from {configPath}", ConfigPath);
            try
            {
                Document = TomlParser.ParseFile(ConfigPath);
                return;
            }
            catch (Exception ex)
            {
                Program.MainLogger.LogError(ex, "Failed to parse config file");
            }
        }

        Program.MainLogger.LogInformation("Creating a new config");
        Document ??= TomlDocument.CreateEmpty();
    }

    public static void Save()
    {
        Program.MainLogger.LogInformation("Saving config");
        Saving?.Invoke();
        try
        {
            Directory.CreateDirectory(Program.AppDataDir);

            var doc = Document.SerializedValue;
            File.WriteAllText(ConfigPath, doc);
        }
        catch (Exception ex)
        {
            Program.MainLogger.LogError(ex, "Failed to save config file");
            return;
        }

        Program.MainLogger.LogInformation("Config saved to {configPath}", ConfigPath);
    }
}
