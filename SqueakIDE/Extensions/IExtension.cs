using SqueakIDE;
using SqueakIDE.Extensions;
using System;

namespace SqueakIDE.Extensions;

public interface IExtension
{
    string Name { get; }
    string Description { get; }
    string Version { get; }
    string Author { get; }
    
    void Initialize(IExtensionHost host);
    void Shutdown();
}

public interface IExtensionHost
{
    // Access to main IDE components
    MainWindow MainWindow { get; }
    // Add methods extensions can use
    void RegisterMenuItem(string menuPath, Action action);
    void RegisterToolbarItem(string name, Action action);
    void RegisterCommand(string name, Action<object[]> action);
    // Add events extensions can hook into
    event EventHandler<FileOpenedEventArgs> FileOpened;
    event EventHandler<FileSavedEventArgs> FileSaved;
} 