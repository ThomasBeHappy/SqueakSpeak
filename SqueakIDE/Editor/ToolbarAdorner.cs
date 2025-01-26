using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;

public class ToolbarAdorner : Adorner
{
    private readonly UIElement _toolbar;
    private readonly int _startOffset;

    public ToolbarAdorner(UIElement adornedElement, UIElement toolbar, int startOffset) 
        : base(adornedElement)
    {
        _toolbar = toolbar;
        _startOffset = startOffset;
        AddVisualChild(toolbar);
    }

    protected override int VisualChildrenCount => 1;
    protected override Visual GetVisualChild(int index) => _toolbar;

    protected override Size ArrangeOverride(Size finalSize)
    {
        var textView = AdornedElement as ICSharpCode.AvalonEdit.Rendering.TextView;
        var pos = textView.GetVisualPosition(
            new TextViewPosition(textView.Document.GetLineByOffset(_startOffset).LineNumber, 1), 
            VisualYPosition.LineBottom);
            
        _toolbar.Arrange(new Rect(new Point(pos.X, pos.Y), _toolbar.DesiredSize));
        return finalSize;
    }
} 