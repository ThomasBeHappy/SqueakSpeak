using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace SqueakIDE.Debugging;
public class DebugVisualizer
{
    private readonly Grid _debugOverlay;
    private readonly Border _debugHighlight;
    private readonly ListView _variablesView;
    private readonly ListView _callStackView;

    public DebugVisualizer(Grid overlay, Border highlight, ListView variables, ListView callStack)
    {
        _debugOverlay = overlay;
        _debugHighlight = highlight;
        _variablesView = variables;
        _callStackView = callStack;
    }

    public void ShowDebugOverlay() => _debugOverlay.Visibility = Visibility.Visible;
    
    public void HighlightCurrentLine(int lineNumber)
    {
        // Calculate position based on line number and editor metrics
        var lineHeight = 20; // Get this from editor
        _debugHighlight.Margin = new Thickness(0, lineNumber * lineHeight, 0, 0);
    }
    
    public void UpdateVariables(IEnumerable<DebugVariable> variables)
    {
        _variablesView.ItemsSource = variables;
    }
    
    public void UpdateCallStack(StackFrame[] callStack)
    {
        _callStackView.ItemsSource = callStack.Select(frame => new
        {
            Method = frame.GetMethod()?.Name ?? "Unknown",
            File = System.IO.Path.GetFileName(frame.GetFileName() ?? "Unknown"),
            Line = frame.GetFileLineNumber()
        });
    }
    
    public void ShowExceptionInfo(Exception exception) => 
        MessageBox.Show(exception.Message, "Debug Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
} 