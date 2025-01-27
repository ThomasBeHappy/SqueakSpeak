using SqueakIDE.Controls;
using SqueakIDE.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SqueakIDE.Dialogs
{
    public partial class GitCommitDialog : ModernWindow
    {
        public string CommitMessage { get; private set; }
        public List<string> SelectedFiles { get; private set; }

        private readonly Dictionary<string, ProjectTreeItem.FileStatus> _changedFiles;

        public GitCommitDialog(Dictionary<string, ProjectTreeItem.FileStatus> changedFiles)
        {
            InitializeComponent();
            _changedFiles = changedFiles;
            PopulateFileList();
        }

        private void PopulateFileList()
        {
            foreach (var file in _changedFiles)
            {
                var item = new CheckBox
                {
                    Content = $"{file.Key} ({file.Value})",
                    IsChecked = true,
                    Tag = file.Key,
                    Foreground = Brushes.White,
                    Margin = new Thickness(5)
                };
                FileList.Children.Add(item);
            }
        }

        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CommitMessageBox.Text))
            {
                MessageBox.Show("Please enter a commit message.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CommitMessage = CommitMessageBox.Text;
            SelectedFiles = FileList.Children.OfType<CheckBox>()
                .Where(cb => cb.IsChecked == true)
                .Select(cb => cb.Tag.ToString())
                .ToList();

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 