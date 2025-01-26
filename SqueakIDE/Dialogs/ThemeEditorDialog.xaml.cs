using System.Windows;
using System.Windows.Media;
using SqueakIDE.Themes;

namespace SqueakIDE.Dialogs
{
    public partial class ThemeEditorDialog : Window
    {
        private Theme _currentTheme;
        private Theme _workingCopy;

        public ThemeEditorDialog(Theme theme = null)
        {
            InitializeComponent();
            _currentTheme = theme ?? new Theme
            {
                Name = "New Theme",
                Description = "Custom theme description",
                IsDark = true
            };
            _workingCopy = CopyTheme(_currentTheme);
            LoadThemeToUI();
        }

        private void LoadThemeToUI()
        {
            ThemeNameBox.Text = _workingCopy.Name;
            DescriptionBox.Text = _workingCopy.Description;
            IsDarkThemeBox.IsChecked = _workingCopy.IsDark;

            // Main Colors
            PrimaryBackgroundPicker.SelectedColor = _workingCopy.PrimaryBackground;
            SecondaryBackgroundPicker.SelectedColor = _workingCopy.SecondaryBackground;
            AccentColorPicker.SelectedColor = _workingCopy.AccentColor;

            // Editor Colors
            EditorBackgroundPicker.SelectedColor = _workingCopy.EditorBackground;
            EditorTextPicker.SelectedColor = _workingCopy.EditorText;
            LineNumbersPicker.SelectedColor = _workingCopy.EditorLineNumbers;
            CurrentLinePicker.SelectedColor = _workingCopy.EditorCurrentLine;
            SelectionPicker.SelectedColor = _workingCopy.EditorSelection;

            // Syntax Colors
            KeywordColorPicker.SelectedColor = _workingCopy.KeywordColor;
            StringColorPicker.SelectedColor = _workingCopy.StringColor;
            CommentColorPicker.SelectedColor = _workingCopy.CommentColor;
            NumberColorPicker.SelectedColor = _workingCopy.NumberColor;
            OperatorColorPicker.SelectedColor = _workingCopy.OperatorColor;
        }

        private Theme CopyTheme(Theme source)
        {
            return new Theme
            {
                Name = source.Name,
                Description = source.Description,
                IsDark = source.IsDark,
                
                // Main Colors
                PrimaryBackground = source.PrimaryBackground,
                SecondaryBackground = source.SecondaryBackground,
                AccentColor = source.AccentColor,
                ForegroundColor = source.ForegroundColor,
                
                // Menu Colors
                MenuBackground = source.MenuBackground,
                MenuForeground = source.MenuForeground,
                MenuHighlight = source.MenuHighlight,
                
                // Button Colors
                ButtonBackground = source.ButtonBackground,
                ButtonForeground = source.ButtonForeground,
                ButtonHover = source.ButtonHover,
                ButtonPressed = source.ButtonPressed,
                
                // Selection Colors
                SelectionBackground = source.SelectionBackground,
                SelectionForeground = source.SelectionForeground,
                
                // Border and Focus
                BorderColor = source.BorderColor,
                FocusBorder = source.FocusBorder,
                
                // Status Colors
                ErrorColor = source.ErrorColor,
                WarningColor = source.WarningColor,
                SuccessColor = source.SuccessColor,
                
                // Editor Colors
                EditorBackground = source.EditorBackground,
                EditorText = source.EditorText,
                EditorLineNumbers = source.EditorLineNumbers,
                EditorCurrentLine = source.EditorCurrentLine,
                EditorSelection = source.EditorSelection,
                LineNumberColor = source.LineNumberColor,
                CurrentLineBackground = source.CurrentLineBackground,
                IndentationGuidesColor = source.IndentationGuidesColor,
                
                // Syntax Colors
                KeywordColor = source.KeywordColor,
                StringColor = source.StringColor,
                CommentColor = source.CommentColor,
                NumberColor = source.NumberColor,
                OperatorColor = source.OperatorColor,
                
                // Text Colors
                PrimaryText = source.PrimaryText,
                SecondaryText = source.SecondaryText
            };
        }

        private void UpdateThemeFromUI()
        {
            _workingCopy.Name = ThemeNameBox.Text;
            _workingCopy.Description = DescriptionBox.Text;
            _workingCopy.IsDark = IsDarkThemeBox.IsChecked ?? false;

            // Main Colors
            _workingCopy.PrimaryBackground = PrimaryBackgroundPicker.SelectedColor ?? Colors.Black;
            _workingCopy.SecondaryBackground = SecondaryBackgroundPicker.SelectedColor ?? Colors.DarkGray;
            _workingCopy.AccentColor = AccentColorPicker.SelectedColor ?? Colors.Blue;
            _workingCopy.ForegroundColor = _workingCopy.IsDark ? Color.FromRgb(240, 240, 240) : Color.FromRgb(70, 60, 50);

            // Menu Colors
            _workingCopy.MenuBackground = _workingCopy.PrimaryBackground;
            _workingCopy.MenuForeground = _workingCopy.ForegroundColor;
            _workingCopy.MenuHighlight = _workingCopy.SecondaryBackground;

            // Button Colors
            _workingCopy.ButtonBackground = _workingCopy.SecondaryBackground;
            _workingCopy.ButtonForeground = _workingCopy.ForegroundColor;
            _workingCopy.ButtonHover = _workingCopy.IsDark ? 
                Color.FromRgb((byte)(_workingCopy.SecondaryBackground.R + 20), 
                             (byte)(_workingCopy.SecondaryBackground.G + 20), 
                             (byte)(_workingCopy.SecondaryBackground.B + 20)) :
                Color.FromRgb((byte)(_workingCopy.SecondaryBackground.R - 20),
                             (byte)(_workingCopy.SecondaryBackground.G - 20),
                             (byte)(_workingCopy.SecondaryBackground.B - 20));
            _workingCopy.ButtonPressed = _workingCopy.IsDark ?
                Color.FromRgb((byte)(_workingCopy.SecondaryBackground.R + 40),
                             (byte)(_workingCopy.SecondaryBackground.G + 40),
                             (byte)(_workingCopy.SecondaryBackground.B + 40)) :
                Color.FromRgb((byte)(_workingCopy.SecondaryBackground.R - 40),
                             (byte)(_workingCopy.SecondaryBackground.G - 40),
                             (byte)(_workingCopy.SecondaryBackground.B - 40));

            // Selection Colors
            _workingCopy.SelectionBackground = _workingCopy.AccentColor;
            _workingCopy.SelectionForeground = _workingCopy.ForegroundColor;

            // Border and Focus
            _workingCopy.BorderColor = _workingCopy.SecondaryBackground;
            _workingCopy.FocusBorder = _workingCopy.AccentColor;

            // Status Colors
            _workingCopy.ErrorColor = Color.FromRgb(240, 80, 80);
            _workingCopy.WarningColor = Color.FromRgb(240, 180, 80);
            _workingCopy.SuccessColor = _workingCopy.AccentColor;

            // Editor Colors
            _workingCopy.EditorBackground = EditorBackgroundPicker.SelectedColor ?? Colors.Black;
            _workingCopy.EditorText = EditorTextPicker.SelectedColor ?? Colors.White;
            _workingCopy.EditorLineNumbers = LineNumbersPicker.SelectedColor ?? Colors.Gray;
            _workingCopy.EditorCurrentLine = CurrentLinePicker.SelectedColor ?? Colors.DarkGray;
            _workingCopy.EditorSelection = SelectionPicker.SelectedColor ?? Colors.Blue;
            _workingCopy.LineNumberColor = _workingCopy.EditorLineNumbers;
            _workingCopy.CurrentLineBackground = _workingCopy.EditorCurrentLine;
            _workingCopy.IndentationGuidesColor = _workingCopy.IsDark ? 
                Color.FromRgb(60, 60, 60) : Color.FromRgb(230, 230, 230);

            // Syntax Colors
            _workingCopy.KeywordColor = KeywordColorPicker.SelectedColor ?? Colors.Blue;
            _workingCopy.StringColor = StringColorPicker.SelectedColor ?? Colors.Brown;
            _workingCopy.CommentColor = CommentColorPicker.SelectedColor ?? Colors.Green;
            _workingCopy.NumberColor = NumberColorPicker.SelectedColor ?? Colors.DarkCyan;
            _workingCopy.OperatorColor = OperatorColorPicker.SelectedColor ?? Colors.DarkGray;

            // Text Colors
            _workingCopy.PrimaryText = _workingCopy.ForegroundColor;
            _workingCopy.SecondaryText = _workingCopy.IsDark ?
                Color.FromRgb(200, 200, 200) : Color.FromRgb(120, 110, 100);
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            UpdateThemeFromUI();
            ThemeManager.Instance.ApplyTheme(_workingCopy);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UpdateThemeFromUI();
            ThemeManager.Instance.SaveCustomTheme(_workingCopy);
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTheme != null)
                ThemeManager.Instance.ApplyTheme(_currentTheme);
            DialogResult = false;
            Close();
        }
    }
} 