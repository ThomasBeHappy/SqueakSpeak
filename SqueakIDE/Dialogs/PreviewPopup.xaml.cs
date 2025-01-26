using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SqueakIDE.Dialogs
{
    public partial class PreviewPopup : Window
    {
        public PreviewPopup()
        {
            InitializeComponent();
        }

        public void PlaceBelow(FrameworkElement element, int offset)
        {
            var point = element.PointToScreen(new Point(0, 0));
            var position = element.TransformToAncestor(element).Transform(new Point(offset, element.ActualHeight));

            Left = position.X;
            Top = position.Y + 10;
        }

        public void UpdatePreview(string text)
        {
            PreviewEditor.Text = text;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}