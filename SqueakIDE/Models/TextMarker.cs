using ICSharpCode.AvalonEdit.Document;
using System.Windows.Media;


namespace SqueakIDE.Models
{
    public class TextMarker : TextSegment
    {
        public TextMarkerType MarkerType { get; set; }
        public Color MarkerColor { get; set; }

        public TextMarker(int startOffset, int length)
        {
            StartOffset = startOffset;
            Length = length;
            MarkerType = TextMarkerType.SquigglyUnderline;
            MarkerColor = Colors.Red;
        }
    }
}
