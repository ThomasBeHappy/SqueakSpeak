using System.Windows;
using System.Threading.Tasks;
using System;

namespace SqueakIDE.Dialogs
{
    public partial class LiveShareLinkDialog : Window
    {
        public LiveShareLinkDialog(string sessionUrl)
        {
            InitializeComponent();
            LinkTextBox.Text = sessionUrl;
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (await TryCopyToClipboard(LinkTextBox.Text))
            {
                CopyButton.Content = "Copied!";
                await Task.Delay(2000); // Reset button text after 2 seconds
                CopyButton.Content = "Copy Link";
            }
        }

        private async Task<bool> TryCopyToClipboard(string text)
        {
            const int maxAttempts = 3;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    // Try to copy to clipboard
                    await Dispatcher.InvokeAsync(() =>
                    {
                        Clipboard.SetDataObject(text, true);
                    });
                    return true;
                }
                catch (Exception ex)
                {
                    attempts++;
                    if (attempts == maxAttempts)
                    {
                        MessageBox.Show(
                            "Could not copy to clipboard. Please select the text and copy manually.",
                            "Clipboard Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return false;
                    }
                    await Task.Delay(100); // Wait before retrying
                }
            }
            return false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
} 