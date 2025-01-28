using System.Text.Json;
using System.IO;
using System;

namespace SqueakIDE.Settings
{
    public class IDESettings
    {
        // Mouse Trail Settings
        public bool EnableMouseTrail { get; set; } = true;
        public int TrailLength { get; set; } = 8;
        public double TrailOpacity { get; set; } = 0.6;
        public string TrailColor { get; set; } = "#FFB6C1"; // Light pink
        public bool EnableSparkles { get; set; } = false;
        public int SparkleCount { get; set; } = 3;

        // Mouse Mascot Settings
        public bool EnableMascot { get; set; } = true;
        public bool EnableMascotSounds { get; set; } = true;
        public double MascotScale { get; set; } = 1.0;

        private static string SettingsPath => 
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SqueakIDE",
                "settings.json"
            );

        public static IDESettings Load()
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<IDESettings>(json) ?? new IDESettings();
            }
            return new IDESettings();
        }

        public void Save()
        {
            var directory = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
    }
} 