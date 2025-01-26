using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

public class AIPreviewMarker : IBackgroundRenderer
{
    private readonly TextDocument _document;
    public int StartOffset { get; }
    public int Length { get; set; }

    public AIPreviewMarker(TextDocument document, int startOffset)
    {
        _document = document;
        StartOffset = startOffset;
        Length = 0;
    }

    public KnownLayer Layer => KnownLayer.Background;

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        if (Length <= 0) return;

        var builder = new BackgroundGeometryBuilder();
        builder.AddSegment(textView, new TextSegment 
        { 
            StartOffset = StartOffset, 
            Length = Length 
        });

        var geometry = builder.CreateGeometry();
        if (geometry != null)
        {
            drawingContext.DrawGeometry(
                new SolidColorBrush(Color.FromArgb(50, 0, 255, 0)),
                null,
                geometry);
        }
    }
} 