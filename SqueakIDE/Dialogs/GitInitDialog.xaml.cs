using SqueakIDE.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SqueakIDE.Dialogs;
public partial class GitInitDialog : ModernWindow
{
    public bool IsClone => RepoTypeComboBox.SelectedIndex == 0;
    public string CloneUrl => CloneUrlBox.Text;
    public string Username => UsernameBox.Text;
    public string Email => EmailBox.Text;
    public string Password => PasswordBox.Password;
    public bool SkipGit { get; private set; }

    public GitInitDialog()
    {
        InitializeComponent();
        RepoTypeComboBox.SelectedIndex = 0;
    }

    private void RepoTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ClonePanel.Visibility = IsClone ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsClone && string.IsNullOrWhiteSpace(CloneUrl))
        {
            MessageBox.Show("Please enter a repository URL.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email))
        {
            MessageBox.Show("Please fill in username and email.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void SkipButton_Click(object sender, RoutedEventArgs e)
    {
        SkipGit = true;
        DialogResult = true;
        Close();
    }
} 