using System.Windows;

namespace SqueakIDE.Dialogs
{
    public partial class TextInputDialog : Window
    {
        public string Result => InputBox.Text;
        public string Prompt { get; }

        public TextInputDialog(string title, string prompt)
        {
            InitializeComponent();
            Title = title;
            Prompt = prompt;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputBox.Text))
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 