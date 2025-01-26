using System;
using System.Windows;
using System.Windows.Controls;

namespace SqueakIDE.Editor
{
    public partial class AIPreviewToolbar : Canvas
    {
        public event EventHandler AcceptClicked;
        public event EventHandler RejectClicked;

        public AIPreviewToolbar()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            AcceptClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            RejectClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}