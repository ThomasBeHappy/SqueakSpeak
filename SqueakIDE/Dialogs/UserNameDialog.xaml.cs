using System.Windows;

namespace SqueakIDE.Dialogs;
public partial class UserNameDialog : Window
{
    public string Username { get; private set; }

    public UserNameDialog()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(UsernameTextBox.Text))
        {
            Username = UsernameTextBox.Text;
            DialogResult = true;
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
} 