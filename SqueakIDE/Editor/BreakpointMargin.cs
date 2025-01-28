using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace SqueakIDE.Editor
{
    public class BreakpointMargin : AbstractMargin
    {
        private readonly TextEditor _editor;
        public Brush Background { get; set; }
        private readonly HashSet<int> _breakpoints = new();

        public BreakpointMargin(TextEditor editor)
        {
            _editor = editor;
            Width = 20;
            Background = Brushes.Transparent;
        }

        public void ToggleBreakpoint(int line)
        {
            if (_breakpoints.Contains(line))
                _breakpoints.Remove(line);
            else
                _breakpoints.Add(line);
            
            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(Width, 0);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var renderSize = RenderSize;
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, renderSize.Width, renderSize.Height));

            var textView = _editor.TextArea.TextView;
            var lineHeight = textView.DefaultLineHeight;

            foreach (var line in _breakpoints)
            {
                var visualLine = textView.GetVisualLine(line);
                if (visualLine != null)
                {
                    var y = visualLine.VisualTop - textView.VerticalOffset;
                    var radius = 8.0;
                    var center = new Point(Width / 2, y + lineHeight / 2);
                    drawingContext.DrawEllipse(Brushes.Red, null, center, radius, radius);
                }
            }
        }
    }
} 