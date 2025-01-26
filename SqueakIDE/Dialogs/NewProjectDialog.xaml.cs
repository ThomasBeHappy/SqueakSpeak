using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace SqueakIDE.Dialogs
{
    public partial class NewProjectDialog : Window
    {
        public string ProjectName => ProjectNameBox.Text;
        public string ProjectPath => ProjectPathBox.Text;

        public NewProjectDialog()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProjectPathBox.Text = dialog.SelectedPath;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                MessageBox.Show("Please enter a project name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(ProjectPath))
            {
                MessageBox.Show("Please select a project location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
} 