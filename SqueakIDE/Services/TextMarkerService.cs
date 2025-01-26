using System;
using System.Windows;
using ICSharpCode.AvalonEdit;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System.Windows.Media;
using System.Linq;
using SqueakIDE.Models;

namespace SqueakIDE.Services
{
    public class TextMarkerService : IBackgroundRenderer, IVisualLineTransformer
    {
        public KnownLayer Layer => KnownLayer.Selection;
        private readonly TextSegmentCollection<TextMarker> _markers;
        private readonly TextEditor _editor;
        private readonly TextView _textView;

        public TextMarkerService(TextEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _markers = new TextSegmentCollection<TextMarker>(editor.Document);
            _textView = editor.TextArea.TextView;
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (textView == null)
                throw new ArgumentNullException(nameof(textView));

            if (_editor.Document == null || !_markers.Any())
                return;

            foreach (var marker in _markers)
            {
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
                {
                    var markerGeometry = new RectangleGeometry(rect);
                    var markerPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
                    markerPen.Freeze();

                    switch (marker.MarkerType)
                    {
                        case TextMarkerType.SquigglyUnderline:
                            var zigZagPath = CreateZigZagPath(rect);
                            drawingContext.DrawGeometry(null, markerPen, zigZagPath);
                            break;

                        case TextMarkerType.Background:
                            var brush = new SolidColorBrush(marker.MarkerColor) { Opacity = 0.3 };
                            brush.Freeze();
                            drawingContext.DrawRectangle(brush, null, rect);
                            break;
                    }
                }
            }
        }

        public void Transform(ITextRunConstructionContext context, IList<VisualLineElement> elements)
        {
            if (context == null || elements == null || !_markers.Any())
                return;

            var currentOffset = context.VisualLine.FirstDocumentLine.Offset;
            var endOffset = context.VisualLine.LastDocumentLine.EndOffset;

            foreach (var marker in _markers.FindOverlappingSegments(currentOffset, endOffset - currentOffset))
            {
                if (marker.MarkerType == TextMarkerType.Background)
                {
                    foreach (var element in elements)
                    {
                        int elementOffset = context.VisualLine.FirstDocumentLine.Offset + element.RelativeTextOffset;
                        int elementLength = element.DocumentLength;

                        if (marker.StartOffset <= elementOffset + elementLength &&
                            elementOffset <= marker.EndOffset)
                        {
                            element.TextRunProperties.SetForegroundBrush(Brushes.Black);
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            if (!_textView?.VisualLines?.Any() ?? true)
                return;

            foreach (var marker in _markers.ToList())
            {
                _markers.Remove(marker);
            }
            _markers.Clear();
            _textView.Redraw();
        }

        public TextMarker Create(int startOffset, int length)
        {
            var marker = new TextMarker(startOffset, length);
            _markers.Add(marker);
            _editor.TextArea.TextView.InvalidateVisual();
            return marker;
        }

        private StreamGeometry CreateZigZagPath(Rect rect)
        {
            const int zigZagHeight = 2;
            const int zigZagWidth = 4;

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(rect.Left, rect.Bottom), false, false);

                double x = rect.Left;
                while (x < rect.Right)
                {
                    x += zigZagWidth;
                    context.LineTo(new Point(x, rect.Bottom - zigZagHeight), true, true);

                    x += zigZagWidth;
                    context.LineTo(new Point(x, rect.Bottom), true, true);
                }
            }
            geometry.Freeze();
            return geometry;
        }
    }
}
