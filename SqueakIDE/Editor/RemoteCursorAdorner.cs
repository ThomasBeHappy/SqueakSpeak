using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit;

public class RemoteCursorAdorner : System.Windows.Documents.Adorner
{
    private readonly string _username;
    public readonly SolidColorBrush cursorColor;
    private readonly SolidColorBrush _selectionColor;
    private int _offset;
    private int _selectionStart;
    private int _selectionLength;

    public RemoteCursorAdorner(ICSharpCode.AvalonEdit.TextEditor editor, string username, Color cursorColor) 
        : base(editor.TextArea.TextView)
    {
        if (editor == null) throw new ArgumentNullException(nameof(editor));
        if (username == null) throw new ArgumentNullException(nameof(username));

        _username = username;
        this.cursorColor = new SolidColorBrush(cursorColor);
        _selectionColor = new SolidColorBrush(Color.FromArgb(50, cursorColor.R, cursorColor.G, cursorColor.B));
        _offset = 0;
        _selectionStart = 0;
        _selectionLength = 0;
    }

    public void UpdatePosition(int offset, int selectionStart = -1, int selectionLength = 0)
    {
        _offset = offset;
        _selectionStart = selectionStart;
        _selectionLength = selectionLength;
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var textView = AdornedElement as ICSharpCode.AvalonEdit.Rendering.TextView;
        if (textView == null || textView.Document == null) return;

        try
        {
            // Draw selection if exists
            if (_selectionStart >= 0 && _selectionLength > 0)
            {
                var segment = new ICSharpCode.AvalonEdit.Document.TextSegment 
                { 
                    StartOffset = _selectionStart,
                    Length = _selectionLength 
                };

                foreach (var rect in ICSharpCode.AvalonEdit.Rendering.BackgroundGeometryBuilder
                    .GetRectsForSegment(textView, segment))
                {
                    drawingContext.DrawRectangle(_selectionColor, null, rect);
                }
            }

            // Draw cursor
            var location = textView.Document.GetLocation(_offset);
            var pos = textView.GetVisualPosition(new TextViewPosition(location), 
                ICSharpCode.AvalonEdit.Rendering.VisualYPosition.TextMiddle);

            drawingContext.DrawLine(
                new Pen(cursorColor, 2),
                new Point(pos.X, pos.Y - 8),
                new Point(pos.X, pos.Y + 8));

            // Draw username
            var formattedText = new FormattedText(
                _username,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                12,
                cursorColor,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            drawingContext.DrawText(
                formattedText,
                new Point(pos.X + 4, pos.Y - 12));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rendering cursor: {ex}");
        }
    }
} 