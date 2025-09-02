using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Semver;
using SNTools.Game;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace SNTools;

internal static class Program
{
    public static SemVersion Version { get; private set; } = new(0);
    public static ToolsLogger MainLogger { get; } = new("SNTools", Color.Red);
    public static string GameDir { get; private set; } = null!;
    public static string GameExePath { get; private set; } = null!;
    public static string AppDataDir { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SNTools");

    [STAThread]
    private static void Main()
    {
        var versionRaw = typeof(Program).Assembly.GetName().Version!;
        if (versionRaw != null)
            Version = new(versionRaw.Major, versionRaw.Minor, versionRaw.Build, (versionRaw.Revision > 0) ? ["ci", versionRaw.Revision.ToString()] : null);

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

        if (!LoadSteamAPI())
        {
            MainLogger.LogInformation("Failed to load the SteamAPI. Ensure Steam is running in the background.");
            return;
        }

        var unityPlayerPath = Path.Combine(GameDir, "UnityPlayer.dll");

        if (!NativeLibrary.TryLoad(unityPlayerPath, out _))
            return;

        ExeSpoofer.Init();
        ModuleSpoofer.Init();
        if (!Il2CppHijack.Init())
            return;

        Il2CppHijack.ReadyToMod += GameModController.Init;

        UnityPlayer.UnityMain();
    }

    private static bool LoadSteamAPI()
    {
        Environment.SetEnvironmentVariable("SteamAppId", "859570");

        var steamLibPath = Path.Combine(GameDir, "Secret Neighbour_Data", "Plugins", "x86_64", "steam_api64.dll");
        return NativeLibrary.TryLoad(steamLibPath, out _) && SteamAPI_Init();
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

    [DllImport("steam_api64.dll")]
    private static extern bool SteamAPI_Init();
}
