using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;

namespace SqueakIDE.Debugging;
public class CallStackViewer : UserControl
{
    private readonly ListView _stackList;

    public void UpdateCallStack(StackFrame[] frames)
    {
        _stackList.Items.Clear();

        foreach (var frame in frames)
        {
            var item = new ListViewItem
            {
                Content = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new TextBlock { Text = "🕳️" }, // Tunnel icon
                        new TextBlock 
                        { 
                            Text = $"{frame.GetMethod().Name} at {GetFileInfo(frame)}"
                        }
                    }
                }
            };

            _stackList.Items.Add(item);
        }
    }

    private string GetFileInfo(StackFrame frame)
    {
        if (frame.GetFileName() is string fileName)
        {
            return $"{Path.GetFileName(fileName)}:{frame.GetFileLineNumber()}";
        }
        return "<unknown>";
    }
} 