using Microsoft.Extensions.Logging;
using Semver;
using SNTools.Game;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
#if STEAM
using Microsoft.Win32;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
#endif

namespace SNTools;

internal static class Program
{
    public static SemVersion Version { get; private set; } = new(0);
    public static ToolsLogger MainLogger { get; } = new("SNTools", Color.Red);
    public static string GameDir { get; private set; } = null!;
    public static string GameExePath { get; private set; } = null!;
    public static string AppDataDir { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SNTools");
    public static string OurExePath { get; } = Environment.ProcessPath!;

    public static string TargetPlatform { get; } =
#if STEAM
        "Steam";
#elif GDK
        "GDK";
#endif

#if GDK
    public static string GdkConfigCopyPath { get; } = Path.Combine(Path.GetDirectoryName(OurExePath)!, "MicrosoftGame.Config");
#endif

    [STAThread]
    private static void Main()
    {
        var versionRaw = typeof(Program).Assembly.GetName().Version!;
        if (versionRaw != null)
            Version = new(versionRaw.Major, versionRaw.Minor, versionRaw.Build, (versionRaw.Revision > 0) ? ["ci", versionRaw.Revision.ToString()] : null);

        ConsoleHandler.Init($"SNTools {TargetPlatform} v{Version}");

        MainLogger.LogInformation("SNTools {platform} v{v}", TargetPlatform, Version);

        var gamePath = FindGamePath();
        if (gamePath == null)
        {
            MainLogger.LogCritical("Couldn't find the game. Make sure it's installed.");
            return;
        }

        GameExePath = gamePath;
        GameDir = Path.GetDirectoryName(gamePath)!;

        Directory.SetCurrentDirectory(GameDir);

        StartGame();
    }

    private static void StartGame()
    {
        ConsoleHandler.NullHandles();

#if STEAM
        if (!LoadSteamAPI())
        {
            MainLogger.LogCritical("Failed to load the SteamAPI. Ensure Steam is running in the background.");
            return;
        }
#endif

        var unityPlayerPath = Path.Combine(GameDir, "UnityPlayer.dll");

        if (!NativeLibrary.TryLoad(unityPlayerPath, out _))
            return;

        ExeSpoofer.Init();
        ModuleSpoofer.Init();
        if (!Il2CppHijack.Init())
            return;

#if GDK
        Il2CppHijack.ReadyToMod += SetUpGDK;
#endif
        Il2CppHijack.ReadyToMod += ExeSpoofer.Dispose;
        Il2CppHijack.ReadyToMod += GameModController.Init;

        UnityPlayer.UnityMain();
    }

    public static void Quit()
    {
        ToolsConfig.Save();

#if GDK
        try
        {
            File.Delete(GdkConfigCopyPath);
        }
        catch { }
#endif
    }

#if GDK
    private static void SetUpGDK()
    {
        if (File.Exists(GdkConfigCopyPath))
            return;

        var gdkConfigPath = Path.Combine(GameDir, "MicrosoftGame.Config");
        if (!File.Exists(gdkConfigPath))
            return;

        try
        {
            File.Copy(gdkConfigPath, GdkConfigCopyPath);
            File.SetAttributes(GdkConfigCopyPath, File.GetAttributes(GdkConfigCopyPath) | FileAttributes.Hidden | FileAttributes.System);
        }
        catch { }
    }
#endif

#if STEAM
    private static bool LoadSteamAPI()
    {
        Environment.SetEnvironmentVariable("SteamAppId", "859570");

        var steamLibPath = Path.Combine(GameDir, "Secret Neighbour_Data", "Plugins", "x86_64", "steam_api64.dll");
        return NativeLibrary.TryLoad(steamLibPath, out _) && SteamAPI_Init();
    }
#endif

    private static string? FindGamePath()
    {
#if STEAM
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

        var gamePath = Path.Combine(targetSteamAppsDir, "common", gameDirName, "Secret Neighbour.exe");

        return File.Exists(gamePath) ? gamePath : null;
#elif GDK
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.DriveType != DriveType.Fixed || !drive.IsReady)
                continue;

            var path = Path.Combine(drive.RootDirectory.FullName, "XboxGames", "Secret Neighbor", "Content", "Secret Neighbor.exe");

            if (File.Exists(path))
                return path;
        }

        return null;
#endif
    }

#if STEAM
    [DllImport("steam_api64.dll")]
    private static extern bool SteamAPI_Init();
#endif
}
