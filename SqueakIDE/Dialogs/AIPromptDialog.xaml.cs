using System.Windows;

namespace SqueakIDE.Dialogs;
public partial class AIPromptDialog : Window
{
    public string UserPrompt => PromptBox.Text;

    public AIPromptDialog(string context)
    {
        InitializeComponent();
        ContextBox.Text = context;
        PromptBox.Focus();
    }

    private void Generate_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(PromptBox.Text))
        {
            DialogResult = true;
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
} 