using SqueakIDE.Windows;
using System.Windows;

namespace SqueakIDE.Dialogs;
public partial class SearchOptionsDialog : ModernWindow
{
    public SearchOptions Options { get; private set; }

    public SearchOptionsDialog(SearchOptions currentOptions)
    {
        InitializeComponent();
        Options = currentOptions;

        MatchCaseCheckBox.IsChecked = Options.MatchCase;
        WholeWordCheckBox.IsChecked = Options.WholeWord;
        RegexCheckBox.IsChecked = Options.UseRegex;
        SearchAllFilesCheckBox.IsChecked = Options.SearchInAllFiles;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Options.MatchCase = MatchCaseCheckBox.IsChecked ?? false;
        Options.WholeWord = WholeWordCheckBox.IsChecked ?? false;
        Options.UseRegex = RegexCheckBox.IsChecked ?? false;
        Options.SearchInAllFiles = SearchAllFilesCheckBox.IsChecked ?? false;

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}