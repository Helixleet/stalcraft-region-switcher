using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Forms;

namespace STALZONERegionSwitcher
{
    public static class SteamHelper
    {
        public const string APP_ID = "1818450";
        public const string FORCED_REALM_FILE = "sc_forced_realm";

        public static readonly Dictionary<string, string> Regions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "RU", "Россия" },
            { "GLOBAL", "EU/NA/ASIA" }
        };

        public static string FindGameFolder()
        {
            string steamPath = GetSteamInstallPath();
            if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
                return null;

            // Try libraryfolders.vdf first
            string libFoldersPath = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
            if (File.Exists(libFoldersPath))
            {
                try
                {
                    string content = File.ReadAllText(libFoldersPath, Encoding.UTF8);
                    var libraries = ParseLibraryFolders(content);
                    foreach (var lib in libraries)
                    {
                        string candidate = TryGetInstallDirFromLibrary(lib, APP_ID);
                        if (!string.IsNullOrEmpty(candidate))
                            return candidate;
                    }
                }
                catch { }
            }

            // Fallback: default steamapps
            string defaultManifest = Path.Combine(steamPath, "steamapps", "appmanifest_" + APP_ID + ".acf");
            if (File.Exists(defaultManifest))
            {
                string installDir = ParseInstallDir(File.ReadAllText(defaultManifest, Encoding.UTF8));
                if (!string.IsNullOrEmpty(installDir))
                {
                    string candidate = Path.Combine(steamPath, "steamapps", "common", installDir);
                    if (IsValidGameFolder(candidate))
                        return Path.GetFullPath(candidate);
                }
            }

            return null;
        }

        private static string GetSteamInstallPath()
        {
            string[] keys = new[]
            {
                @"SOFTWARE\Valve\Steam",
                @"SOFTWARE\Wow6432Node\Valve\Steam"
            };

            // HKLM
            foreach (var key in keys)
            {
                try
                {
                    using (var reg = Registry.LocalMachine.OpenSubKey(key))
                    {
                        if (reg != null)
                        {
                            object val = reg.GetValue("InstallPath");
                            if (val != null)
                            {
                                string path = val.ToString();
                                if (Directory.Exists(path))
                                    return path;
                            }
                        }
                    }
                }
                catch { }
            }

            // HKCU
            try
            {
                using (var reg = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (reg != null)
                    {
                        object val = reg.GetValue("InstallPath");
                        if (val != null)
                        {
                            string path = val.ToString();
                            if (Directory.Exists(path))
                                return path;
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        private static List<string> ParseLibraryFolders(string vdfContent)
        {
            var result = new List<string>();
            // Simple regex: "path"\s+"(.+?)"
            var matches = Regex.Matches(vdfContent, "\"path\"\\s+\"([^\"]+)\"", RegexOptions.IgnoreCase);
            foreach (Match m in matches)
            {
                string p = m.Groups[1].Value.Replace("\\\\", "\\");
                if (Directory.Exists(p))
                    result.Add(p);
            }
            return result;
        }

        private static string TryGetInstallDirFromLibrary(string libraryPath, string appId)
        {
            string manifest = Path.Combine(libraryPath, "steamapps", "appmanifest_" + appId + ".acf");
            if (!File.Exists(manifest))
                return null;

            string installDir = ParseInstallDir(File.ReadAllText(manifest, Encoding.UTF8));
            if (string.IsNullOrEmpty(installDir))
                return null;

            string candidate = Path.Combine(libraryPath, "steamapps", "common", installDir);
            if (IsValidGameFolder(candidate))
                return Path.GetFullPath(candidate);

            return null;
        }

        private static string ParseInstallDir(string acfContent)
        {
            var m = Regex.Match(acfContent, "\"installdir\"\\s+\"([^\"]+)\"", RegexOptions.IgnoreCase);
            if (m.Success)
                return m.Groups[1].Value;
            return null;
        }

        public static bool IsValidGameFolder(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return false;

            string appidFile = Path.Combine(path, "steam_appid.txt");
            if (!File.Exists(appidFile))
                return false;

            try
            {
                string content = File.ReadAllText(appidFile, Encoding.UTF8).Trim();
                return content == APP_ID;
            }
            catch
            {
                return false;
            }
        }

        public static void StartGame()
        {
            string url = "steam://rungameid/" + APP_ID;
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось запустить игру\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string GetCurrentRegion(string gameFolder)
        {
            if (string.IsNullOrEmpty(gameFolder))
                return "не задан";

            string forced = Path.Combine(gameFolder, FORCED_REALM_FILE);
            if (!File.Exists(forced))
                return "не задан";

            try
            {
                string val = File.ReadAllText(forced, Encoding.UTF8).Trim().ToUpperInvariant();
                string name;
                if (Regions.TryGetValue(val, out name))
                    return name;
                return "неизвестно (" + val + ")";
            }
            catch
            {
                return "ошибка чтения";
            }
        }

        public static void SetRegion(string gameFolder, string regionCode)
        {
            string forced = Path.Combine(gameFolder, FORCED_REALM_FILE);

            if (string.IsNullOrEmpty(regionCode) || regionCode == "AUTO")
            {
                if (File.Exists(forced))
                    File.Delete(forced);
            }
            else
            {
                File.WriteAllText(forced, regionCode, new UTF8Encoding(false));
            }
        }
    }
}
