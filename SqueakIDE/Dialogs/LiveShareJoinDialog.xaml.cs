using System.Windows;

namespace SqueakIDE.Dialogs
{
    public partial class LiveShareJoinDialog : Window
    {
        public string SessionUrl => SessionUrlTextBox.Text;

        public LiveShareJoinDialog()
        {
            InitializeComponent();
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SessionUrl))
            {
                MessageBox.Show("Please enter a session URL", "Invalid URL", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 