using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using SqueakIDE;

namespace SqueakIDE.Extensions;
public class ExtensionHost : IExtensionHost
{
    private readonly MainWindow _mainWindow;
    MainWindow IExtensionHost.MainWindow => _mainWindow;

    event EventHandler<FileOpenedEventArgs> IExtensionHost.FileOpened
    {
        add { FileOpened += value; }
        remove { FileOpened -= value; }
    }

    event EventHandler<FileSavedEventArgs> IExtensionHost.FileSaved
    {
        add { FileSaved += value; }
        remove { FileSaved -= value; }
    }

    private event EventHandler<FileOpenedEventArgs> FileOpened;
    private event EventHandler<FileSavedEventArgs> FileSaved;

    public ExtensionHost(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    void IExtensionHost.RegisterMenuItem(string menuPath, Action action)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var pathParts = menuPath.Split('/');
            var currentMenu = FindOrCreateMenu("Extensions");
            
            // Navigate/create menu hierarchy
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                var menuItem = FindOrCreateMenuItem(currentMenu.Items, pathParts[i]);
                currentMenu = menuItem;
            }

            // Add final menu item
            var newItem = new MenuItem
            {
                Header = pathParts[^1]
            };
            newItem.Click += (s, e) => action();
            currentMenu.Items.Add(newItem);
        });
    }

    void IExtensionHost.RegisterToolbarItem(string name, Action action)
    {
        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
        {
            var button = new Button 
            { 
                Content = name,
                Style = Application.Current.FindResource("ToolBarButtonStyle") as Style
            };
            button.Click += (s, e) => action();
            if (_mainWindow.FindName("ExtensionsToolbar") is StackPanel extensionsPanel)
            {
                extensionsPanel.Children.Add(button);
            }
        }));
    }

    void IExtensionHost.RegisterCommand(string name, Action<object[]> action)
    {
        CommandRegistry.Register(name, action);
    }

    private MenuItem FindOrCreateMenu(string header)
    {
        if (_mainWindow.FindName("MainMenu") is Menu mainMenu)
        {
            foreach (MenuItem item in mainMenu.Items)
            {
                if (item.Header.ToString() == header)
                    return item;
            }

            var newMenu = new MenuItem { Header = header };
            mainMenu.Items.Add(newMenu);
            return newMenu;
        }
        throw new InvalidOperationException("MainMenu not found in MainWindow");
    }

    private MenuItem FindOrCreateMenuItem(ItemCollection items, string header)
    {
        foreach (var item in items.OfType<MenuItem>())
        {
            if (item.Header.ToString() == header)
                return item;
        }

        var newItem = new MenuItem { Header = header };
        items.Add(newItem);
        return newItem;
    }

    internal void OnFileOpened(string filePath)
    {
        FileOpened?.Invoke(this, new FileOpenedEventArgs(filePath));
    }

    internal void OnFileSaved(string filePath)
    {
        FileSaved?.Invoke(this, new FileSavedEventArgs(filePath));
    }
}

public class FileOpenedEventArgs : EventArgs
{
    public string FilePath { get; }
    public FileOpenedEventArgs(string filePath) => FilePath = filePath;
}

public class FileSavedEventArgs : EventArgs
{
    public string FilePath { get; }
    public FileSavedEventArgs(string filePath) => FilePath = filePath;
} 