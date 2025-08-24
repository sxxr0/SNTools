using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Win32;
using Semver;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SNTools;

internal static class Program
{
    public static SemVersion Version { get; private set; } = new SemVersion(0);
    public static ToolsLogger MainLogger { get; } = new("SNTools", Color.Red);
    public static string GameDir { get; private set; } = null!;
    public static string GameExePath { get; private set; } = null!;

    private static void Main()
    {
        var versionRaw = FileVersionInfo.GetVersionInfo(Environment.ProcessPath!).ProductVersion;
        if (SemVersion.TryParse(versionRaw, SemVersionStyles.Any, out var version))
            Version = version.WithoutMetadata();

        ConsoleHandler.Init($"SNTools v{Version}");

        var gameDir = FindGameDir();
        if (gameDir == null)
            return;

        GameDir = gameDir;
        GameExePath = Path.Combine(gameDir, "Secret Neighbour.exe");

        if (!File.Exists(GameExePath))
            return;

        Directory.SetCurrentDirectory(gameDir);

        StartGame();
    }

    private static void StartGame()
    {
        ConsoleHandler.NullHandles();

        var unityPlayerPath = Path.Combine(GameDir, "UnityPlayer.dll");

        if (!NativeLibrary.TryLoad(unityPlayerPath, out _))
            return;

        ExeSpoofer.Init();
        ModuleSpoofer.Init();
        if (!Il2CppHijack.Init())
            return;

        Il2CppHijack.ReadyToMod += GameModController.Init;

        Environment.SetEnvironmentVariable("SteamAppId", "859570");

        UnityPlayer.UnityMain();
    }

    private static string? FindGameDir()
    {
        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam") ?? Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Valve\Steam");
        var steamPath = (string?)key?.GetValue("InstallPath");

        if (!Directory.Exists(steamPath))
            return null;

        var libVdfPath = Path.Combine(steamPath, "config", "libraryfolders.vdf");
        if (!File.Exists(libVdfPath))
            return null;

        var libFoldersVdf = VdfConvert.Deserialize(File.ReadAllText(libVdfPath));
        var libDirs = libFoldersVdf.Value.Select(x => ((VProperty)((VProperty)x).Value.First(y => ((VProperty)y).Key == "path")).Value.ToString());

        var targetAcfName = "appmanifest_859570.acf";

        var targetLibDir = libDirs.FirstOrDefault(x => File.Exists(Path.Combine(x, "steamapps", targetAcfName)));
        if (targetLibDir == null)
            return null;

        var targetSteamAppsDir = Path.Combine(targetLibDir, "steamapps");
        var manifestPath = Path.Combine(targetSteamAppsDir, targetAcfName);

        var manifestAcf = VdfConvert.Deserialize(File.ReadAllText(manifestPath));
        var gameDirName = ((VProperty?)manifestAcf.Value.FirstOrDefault(x => ((VProperty)x).Key == "installdir"))?.Value?.ToString();
        if (gameDirName == null)
            return null;

        var gameDir = Path.Combine(targetSteamAppsDir, "common", gameDirName);

        return Directory.Exists(gameDir) ? gameDir : null;
    }
}
