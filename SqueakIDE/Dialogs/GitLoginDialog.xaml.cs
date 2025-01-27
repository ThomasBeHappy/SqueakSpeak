using SqueakIDE.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SqueakIDE.Dialogs;
public partial class GitLoginDialog : ModernWindow
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public GitLoginDialog()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameBox.Text) ||
            string.IsNullOrWhiteSpace(EmailBox.Text) ||
            string.IsNullOrWhiteSpace(PasswordBox.Password))
        {
            MessageBox.Show("Please fill in all fields.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Username = UsernameBox.Text;
        Email = EmailBox.Text;
        Password = PasswordBox.Password;

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
} 