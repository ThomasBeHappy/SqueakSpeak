using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SqueakIDE.Debugging;
public class VariableInspector : UserControl
{
    private readonly TreeView _variableTree;
    private readonly Dictionary<Type, string> _typeIcons = new Dictionary<Type, string>
    {
        { typeof(int), "🧀" },    // Simple types
        { typeof(string), "🧀" },
        { typeof(List<>), "🧀" },  // Collections
        { typeof(Exception), "⚠️" } // Exceptions
    };

    public void UpdateVariables(IEnumerable<DebugVariable> variables)
    {
        _variableTree.Items.Clear();

        foreach (var variable in variables)
        {
            var item = new TreeViewItem
            {
                Header = CreateVariableHeader(variable),
                Tag = variable
            };

            if (HasChildren(variable))
            {
                item.Items.Add(new TreeViewItem { Header = "Loading..." });
                item.Expanded += OnVariableExpanded;
            }

            _variableTree.Items.Add(item);
        }
    }

    private StackPanel CreateVariableHeader(DebugVariable variable)
    {
        return new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Children =
            {
                new TextBlock { Text = GetTypeIcon(variable.Type) },
                new TextBlock { Text = variable.Name },
                new TextBlock { Text = " = " },
                new TextBlock { Text = FormatValue(variable.Value) }
            }
        };
    }

    private string GetTypeIcon(Type type)
    {
        return _typeIcons.TryGetValue(type, out var icon) ? icon : "❓";
    }

    private bool HasChildren(DebugVariable variable)
    {
        // Implementation of HasChildren method
        return false; // Placeholder return, actual implementation needed
    }

    private void OnVariableExpanded(object sender, RoutedEventArgs e)
    {
        // Implementation of OnVariableExpanded method
    }

    private string FormatValue(object value)
    {
        // Implementation of FormatValue method
        return "N/A"; // Placeholder return, actual implementation needed
    }
} 