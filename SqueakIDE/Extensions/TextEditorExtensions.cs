using ICSharpCode.AvalonEdit;
using System.Windows;

namespace SqueakIDE.Extensions
{
    public static class TextEditorExtensions
    {
        public static int GetLineFromMousePosition(this TextEditor editor, Point position)
        {
            var pos = editor.TextArea.TextView.GetPosition(position);
            return pos?.Line ?? -1;
        }
    }
}