using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace SqueakIDE.Extensions;
public class ExtensionManager
{
    private readonly Dictionary<string, IExtension> _loadedExtensions = new();
    private readonly ExtensionHost _host;

    public ExtensionManager(ExtensionHost host)
    {
        _host = host;
    }

    public void LoadExtensions(string extensionsDirectory)
    {
        // Load DLLs from extensions directory
        foreach (var dllPath in Directory.GetFiles(extensionsDirectory, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IExtension).IsAssignableFrom(type) && !type.IsInterface)
                    {
                        var extension = (IExtension)Activator.CreateInstance(type);
                        extension.Initialize(_host);
                        _loadedExtensions[extension.Name] = extension;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error loading extension
                Debug.WriteLine($"Error loading extension from {dllPath}: {ex.Message}");
            }
        }
    }
} 