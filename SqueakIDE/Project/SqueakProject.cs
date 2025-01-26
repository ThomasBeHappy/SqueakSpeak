using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace SqueakIDE.Project
{
    public class SqueakProject
    {
        public string Name { get; set; }
        public string RootPath { get; set; }
        public List<string> SourceFiles { get; set; } = new();
        public List<string> References { get; set; } = new();
        public Dictionary<string, string> Settings { get; set; } = new();

        public static SqueakProject Create(string name, string path)
        {
            return new SqueakProject
            {
                Name = name,
                RootPath = path,
                Settings = new Dictionary<string, string>
                {
                    { "OutputType", "Executable" },
                    { "Language", "SqueakSpeak" }
                }
            };
        }

        public void Save()
        {
            var projectFile = Path.Combine(RootPath, $"{Name}.squeakproj");
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(projectFile, json);
        }

        public static SqueakProject Load(string projectFile)
        {
            var json = File.ReadAllText(projectFile);
            return JsonSerializer.Deserialize<SqueakProject>(json);
        }

        public void AddSourceFile(string filePath)
        {
            var relativePath = Path.GetRelativePath(RootPath, filePath);
            if (!SourceFiles.Contains(relativePath))
            {
                SourceFiles.Add(relativePath);
                Save();
            }
        }

        public void RemoveSourceFile(string filePath)
        {
            var relativePath = Path.GetRelativePath(RootPath, filePath);
            if (SourceFiles.Contains(relativePath))
            {
                SourceFiles.Remove(relativePath);
                Save();
            }
        }
    }
} 