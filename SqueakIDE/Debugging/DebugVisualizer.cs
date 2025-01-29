using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using ICSharpCode.AvalonEdit;

namespace SqueakIDE.Debugging;
public class DebugVisualizer
{
    private readonly Grid _debugOverlay;
    private readonly Border _debugHighlight;
    private readonly ListView _variablesView;
    private readonly ListView _callStackView;
    private ICSharpCode.AvalonEdit.TextEditor _currentEditor;

    public DebugVisualizer(Grid overlay, Border highlight, ListView variables, ListView callStack)
    {
        _debugOverlay = overlay;
        _debugHighlight = highlight;
        _variablesView = variables;
        _callStackView = callStack;

        // Style the debug highlight
        _debugHighlight.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 0));  // More visible yellow
        _debugHighlight.BorderBrush = new SolidColorBrush(Colors.Orange);  // Add border
        _debugHighlight.BorderThickness = new Thickness(2);
        _debugHighlight.HorizontalAlignment = HorizontalAlignment.Stretch;
        _debugHighlight.VerticalAlignment = VerticalAlignment.Top;
        _debugHighlight.Visibility = Visibility.Collapsed;  // Start hidden
    }

    public void SetCurrentEditor(ICSharpCode.AvalonEdit.TextEditor editor)
    {
        _debugHighlight.Dispatcher.Invoke(() => _currentEditor = editor);
    }

    public void ShowDebugOverlay()
    {
        _debugOverlay.Dispatcher.Invoke(() => 
        {
            _debugOverlay.Visibility = Visibility.Visible;
            _debugHighlight.Visibility = Visibility.Visible;
        });
    }

    public void HideDebugOverlay()
    {
        _debugOverlay.Dispatcher.Invoke(() => 
        {
            _debugOverlay.Visibility = Visibility.Collapsed;
            _debugHighlight.Visibility = Visibility.Collapsed;
        });
    }

    public void HighlightCurrentLine(int lineNumber)
    {
        var editor = _currentEditor;
        _debugHighlight.Dispatcher.Invoke(() => 
        {
            if (editor != null)
            {
                editor.ScrollTo(lineNumber, 0);
                
                // Get editor position in window coordinates
                var editorPos = editor.TransformToVisual(Application.Current.MainWindow).Transform(new Point(0, 0));
                var linePos = editor.TextArea.TextView.GetVisualTopByDocumentLine(lineNumber);
                var lineHeight = editor.TextArea.TextView.DefaultLineHeight;
                
                Canvas.SetLeft(_debugHighlight, editorPos.X);
                Canvas.SetTop(_debugHighlight, editorPos.Y + linePos);
                _debugHighlight.Width = editor.ActualWidth;
                _debugHighlight.Height = lineHeight;
                _debugHighlight.Visibility = Visibility.Visible;
            }
        });
    }
    
    public void UpdateVariables(IEnumerable<DebugVariable> variables)
    {
        _variablesView.Dispatcher.Invoke(() => 
            _variablesView.ItemsSource = variables);
    }
    
    public void UpdateCallStack(StackFrame[] callStack)
    {
        _callStackView.Dispatcher.Invoke(() => 
            _callStackView.ItemsSource = callStack.Select(frame => new
            {
                Method = frame.GetMethod()?.Name ?? "Unknown",
                File = System.IO.Path.GetFileName(frame.GetFileName() ?? "Unknown"),
                Line = frame.GetFileLineNumber()
            }));
    }
    
    public void ShowExceptionInfo(Exception exception) => 
        _debugOverlay.Dispatcher.Invoke(() =>
            MessageBox.Show(exception.Message, "Debug Exception", MessageBoxButton.OK, MessageBoxImage.Warning));
} 