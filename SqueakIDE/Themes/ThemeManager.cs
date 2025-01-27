using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Linq;
using System.Windows.Media;

namespace SqueakIDE.Themes
{
    public class ThemeManager
    {
        private static ThemeManager _instance;
        public static ThemeManager Instance => _instance ??= new ThemeManager();

        private Dictionary<string, Theme> _themes = new();
        public Theme CurrentTheme { get; private set; }
        
        private string ThemesDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SqueakIDE",
            "Themes"
        );

        private ThemeManager()
        {
            InitializeDefaultThemes();
            LoadCustomThemes();
            // Set a default theme if none is selected
            if (CurrentTheme == null)
            {
                ApplyTheme("CozyDark");
            }
        }

        private void InitializeDefaultThemes()
        {
            try
            {
                // Initialize other themes first
                _themes["CheeseCave"] = new Theme
                {
                    Name = "Cheese Cave",
                    Description = "A warm, cheese-inspired light theme",
                    IsDark = false,
                    PrimaryBackground = Color.FromRgb(255, 253, 240),
                    SecondaryBackground = Color.FromRgb(255, 248, 220),
                    TertiaryBackground = Color.FromRgb(255, 245, 215),
                    InputBackground = Color.FromRgb(255, 250, 230),
                    HoverBackground = Color.FromRgb(255, 235, 200),
                    AccentColor = Color.FromRgb(255, 183, 77),
                    ForegroundColor = Color.FromRgb(70, 60, 50),
                    MenuBackground = Color.FromRgb(255, 248, 220),
                    MenuForeground = Color.FromRgb(70, 60, 50),
                    MenuHighlight = Color.FromRgb(255, 235, 200),
                    ButtonBackground = Color.FromRgb(255, 248, 220),
                    ButtonForeground = Color.FromRgb(70, 60, 50),
                    ButtonHover = Color.FromRgb(255, 235, 200),
                    ButtonPressed = Color.FromRgb(255, 223, 186),
                    SelectionBackground = Color.FromRgb(255, 223, 186),
                    SelectionForeground = Color.FromRgb(70, 60, 50),
                    FocusBorder = Color.FromRgb(255, 183, 77),
                    ErrorColor = Color.FromRgb(220, 50, 50),
                    WarningColor = Color.FromRgb(230, 150, 50),
                    SuccessColor = Color.FromRgb(80, 160, 80),
                    LineNumberColor = Color.FromRgb(180, 170, 160),
                    CurrentLineBackground = Color.FromRgb(255, 248, 220),
                    IndentationGuidesColor = Color.FromRgb(230, 220, 210),
                    EditorBackground = Color.FromRgb(255, 253, 240),
                    EditorText = Color.FromRgb(70, 60, 50),
                    EditorLineNumbers = Color.FromRgb(180, 170, 160),
                    EditorCurrentLine = Color.FromRgb(255, 248, 220),
                    EditorSelection = Color.FromRgb(255, 223, 186),
                    KeywordColor = Color.FromRgb(175, 50, 100),
                    StringColor = Color.FromRgb(160, 90, 40),
                    CommentColor = Color.FromRgb(100, 140, 80),
                    OperatorColor = Color.FromRgb(120, 110, 100),
                    NumberColor = Color.FromRgb(140, 120, 40),
                    BorderColor = Color.FromRgb(230, 220, 210),
                    PrimaryText = Color.FromRgb(70, 60, 50),
                    SecondaryText = Color.FromRgb(120, 110, 100),
                    ActiveBackground = Color.FromRgb(255, 240, 210),
                    PopupBackground = Color.FromRgb(255, 250, 235),
                    HoverForeground = Color.FromRgb(70, 60, 50),
                    ActiveForeground = Color.FromRgb(70, 60, 50)
                };

                _themes["OceanDepths"] = new Theme
                {
                    Name = "Ocean Depths",
                    Description = "Deep blues and aqua accents for a sea-inspired experience",
                    IsDark = true,
                    PrimaryBackground = Color.FromRgb(15, 25, 35),
                    SecondaryBackground = Color.FromRgb(25, 45, 65),
                    TertiaryBackground = Color.FromRgb(30, 50, 70),
                    InputBackground = Color.FromRgb(20, 35, 50),
                    HoverBackground = Color.FromRgb(35, 60, 85),
                    AccentColor = Color.FromRgb(64, 224, 208),
                    ForegroundColor = Color.FromRgb(220, 230, 240),
                    MenuBackground = Color.FromRgb(18, 35, 50),
                    MenuForeground = Color.FromRgb(220, 230, 240),
                    MenuHighlight = Color.FromRgb(25, 50, 70),
                    ButtonBackground = Color.FromRgb(20, 40, 60),
                    ButtonForeground = Color.FromRgb(220, 230, 240),
                    ButtonHover = Color.FromRgb(30, 60, 85),
                    ButtonPressed = Color.FromRgb(35, 70, 100),
                    SelectionBackground = Color.FromRgb(40, 80, 120),
                    SelectionForeground = Color.FromRgb(240, 250, 255),
                    FocusBorder = Color.FromRgb(64, 224, 208),
                    ErrorColor = Color.FromRgb(255, 85, 85),
                    WarningColor = Color.FromRgb(255, 180, 100),
                    SuccessColor = Color.FromRgb(100, 255, 200),
                    LineNumberColor = Color.FromRgb(100, 140, 180),
                    CurrentLineBackground = Color.FromRgb(18, 36, 54),
                    IndentationGuidesColor = Color.FromRgb(40, 60, 80),
                    EditorBackground = Color.FromRgb(15, 25, 35),
                    EditorText = Color.FromRgb(220, 230, 240),
                    EditorLineNumbers = Color.FromRgb(100, 140, 180),
                    EditorCurrentLine = Color.FromRgb(18, 36, 54),
                    EditorSelection = Color.FromRgb(40, 80, 120),
                    KeywordColor = Color.FromRgb(102, 217, 239),
                    StringColor = Color.FromRgb(230, 219, 116),
                    CommentColor = Color.FromRgb(117, 160, 190),
                    OperatorColor = Color.FromRgb(249, 38, 114),
                    NumberColor = Color.FromRgb(174, 129, 255),
                    BorderColor = Color.FromRgb(35, 55, 75),
                    PrimaryText = Color.FromRgb(220, 230, 240),
                    SecondaryText = Color.FromRgb(180, 200, 220),
                    ActiveBackground = Color.FromRgb(30, 55, 75),
                    PopupBackground = Color.FromRgb(18, 35, 50),
                    HoverForeground = Color.FromRgb(240, 250, 255),
                    ActiveForeground = Color.FromRgb(240, 250, 255)
                };

                _themes["ForestCanopy"] = new Theme
                {
                    Name = "Forest Canopy",
                    Description = "Earthy greens and browns for a natural coding environment",
                    IsDark = true,
                    PrimaryBackground = Color.FromRgb(28, 32, 26),
                    SecondaryBackground = Color.FromRgb(38, 42, 36),
                    TertiaryBackground = Color.FromRgb(43, 47, 41),
                    InputBackground = Color.FromRgb(33, 37, 31),
                    HoverBackground = Color.FromRgb(58, 62, 56),
                    AccentColor = Color.FromRgb(144, 190, 109),
                    ForegroundColor = Color.FromRgb(230, 235, 225),
                    MenuBackground = Color.FromRgb(33, 37, 31),
                    MenuForeground = Color.FromRgb(230, 235, 225),
                    MenuHighlight = Color.FromRgb(48, 52, 46),
                    ButtonBackground = Color.FromRgb(38, 42, 36),
                    ButtonForeground = Color.FromRgb(230, 235, 225),
                    ButtonHover = Color.FromRgb(58, 62, 56),
                    ButtonPressed = Color.FromRgb(68, 72, 66),
                    SelectionBackground = Color.FromRgb(70, 90, 65),
                    SelectionForeground = Color.FromRgb(240, 245, 235),
                    FocusBorder = Color.FromRgb(144, 190, 109),
                    ErrorColor = Color.FromRgb(255, 100, 92),
                    WarningColor = Color.FromRgb(255, 190, 100),
                    SuccessColor = Color.FromRgb(144, 190, 109),
                    LineNumberColor = Color.FromRgb(120, 140, 110),
                    CurrentLineBackground = Color.FromRgb(33, 37, 31),
                    IndentationGuidesColor = Color.FromRgb(58, 62, 56),
                    EditorBackground = Color.FromRgb(28, 32, 26),
                    EditorText = Color.FromRgb(230, 235, 225),
                    EditorLineNumbers = Color.FromRgb(120, 140, 110),
                    EditorCurrentLine = Color.FromRgb(33, 37, 31),
                    EditorSelection = Color.FromRgb(70, 90, 65),
                    KeywordColor = Color.FromRgb(190, 150, 100),
                    StringColor = Color.FromRgb(144, 190, 109),
                    CommentColor = Color.FromRgb(100, 130, 90),
                    OperatorColor = Color.FromRgb(200, 180, 140),
                    NumberColor = Color.FromRgb(170, 200, 140),
                    BorderColor = Color.FromRgb(38, 42, 36),
                    PrimaryText = Color.FromRgb(230, 235, 225),
                    SecondaryText = Color.FromRgb(180, 190, 170),
                    ActiveBackground = Color.FromRgb(48, 52, 46),
                    PopupBackground = Color.FromRgb(33, 37, 31),
                    HoverForeground = Color.FromRgb(240, 245, 235),
                    ActiveForeground = Color.FromRgb(240, 245, 235)
                };

                // Initialize CozyDark last
                _themes["CozyDark"] = new Theme
                {
                    Name = "Cozy Dark",
                    Description = "A warm, dark theme for late-night coding",
                    IsDark = true,
                    PrimaryBackground = Color.FromRgb(30, 30, 30),
                    SecondaryBackground = Color.FromRgb(45, 45, 45),
                    TertiaryBackground = Color.FromRgb(50, 50, 50),
                    InputBackground = Color.FromRgb(40, 40, 40),
                    HoverBackground = Color.FromRgb(60, 60, 60),
                    SelectionBackground = Color.FromRgb(70, 90, 70),
                    ActiveBackground = Color.FromRgb(55, 55, 55),
                    AccentColor = Color.FromRgb(137, 209, 133),
                    ForegroundColor = Color.FromRgb(240, 240, 240),
                    MenuBackground = Color.FromRgb(35, 35, 35),
                    MenuForeground = Color.FromRgb(240, 240, 240),
                    MenuHighlight = Color.FromRgb(60, 60, 60),
                    ButtonBackground = Color.FromRgb(45, 45, 45),
                    ButtonForeground = Color.FromRgb(240, 240, 240),
                    ButtonHover = Color.FromRgb(60, 60, 60),
                    ButtonPressed = Color.FromRgb(70, 70, 70),
                    SelectionForeground = Color.FromRgb(255, 255, 255),
                    FocusBorder = Color.FromRgb(137, 209, 133),
                    ErrorColor = Color.FromRgb(240, 80, 80),
                    WarningColor = Color.FromRgb(240, 180, 80),
                    SuccessColor = Color.FromRgb(137, 209, 133),
                    LineNumberColor = Color.FromRgb(160, 160, 160),
                    CurrentLineBackground = Color.FromRgb(40, 40, 40),
                    IndentationGuidesColor = Color.FromRgb(60, 60, 60),
                    EditorBackground = Color.FromRgb(30, 30, 30),
                    EditorText = Color.FromRgb(240, 240, 240),
                    EditorLineNumbers = Color.FromRgb(160, 160, 160),
                    EditorCurrentLine = Color.FromRgb(40, 40, 40),
                    EditorSelection = Color.FromRgb(70, 90, 70),
                    KeywordColor = Color.FromRgb(197, 134, 192),
                    StringColor = Color.FromRgb(206, 145, 120),
                    CommentColor = Color.FromRgb(106, 153, 85),
                    OperatorColor = Color.FromRgb(200, 200, 200),
                    NumberColor = Color.FromRgb(181, 206, 168),
                    BorderColor = Color.FromRgb(45, 45, 45),
                    PrimaryText = Color.FromRgb(240, 240, 240),
                    SecondaryText = Color.FromRgb(200, 200, 200),
                    PopupBackground = Color.FromRgb(35, 35, 35),
                    HoverForeground = Color.FromRgb(255, 255, 255),
                    ActiveForeground = Color.FromRgb(255, 255, 255)
                };
                // Initialize SolarizedDark
                _themes["SolarizedDark"] = new Theme
                {
                    Name = "Solarized Dark",
                    Description = "The classic Solarized Dark theme",
                    IsDark = true,
                    PrimaryBackground = Color.FromRgb(0, 43, 54),
                    SecondaryBackground = Color.FromRgb(7, 54, 66),
                    TertiaryBackground = Color.FromRgb(0, 43, 54),
                    InputBackground = Color.FromRgb(7, 54, 66),
                    HoverBackground = Color.FromRgb(88, 110, 117),
                    SelectionBackground = Color.FromRgb(7, 54, 66),
                    ActiveBackground = Color.FromRgb(0, 43, 54),
                    PopupBackground = Color.FromRgb(0, 43, 54),
                    AccentColor = Color.FromRgb(38, 139, 210),
                    ForegroundColor = Color.FromRgb(253, 246, 227),
                    MenuBackground = Color.FromRgb(0, 43, 54),
                    MenuForeground = Color.FromRgb(253, 246, 227),
                    MenuHighlight = Color.FromRgb(7, 54, 66),
                    ButtonBackground = Color.FromRgb(7, 54, 66),
                    ButtonForeground = Color.FromRgb(253, 246, 227),
                    ButtonHover = Color.FromRgb(88, 110, 117),
                    ButtonPressed = Color.FromRgb(101, 123, 131),
                    SelectionForeground = Color.FromRgb(253, 246, 227),
                    FocusBorder = Color.FromRgb(38, 139, 210),
                    ErrorColor = Color.FromRgb(220, 50, 47),
                    WarningColor = Color.FromRgb(203, 75, 22),
                    SuccessColor = Color.FromRgb(133, 153, 0),
                    LineNumberColor = Color.FromRgb(101, 123, 131),
                    CurrentLineBackground = Color.FromRgb(7, 54, 66),
                    IndentationGuidesColor = Color.FromRgb(88, 110, 117),
                    EditorBackground = Color.FromRgb(0, 43, 54),
                    EditorText = Color.FromRgb(253, 246, 227),
                    EditorLineNumbers = Color.FromRgb(101, 123, 131),
                    EditorCurrentLine = Color.FromRgb(7, 54, 66),
                    EditorSelection = Color.FromRgb(7, 54, 66),
                    KeywordColor = Color.FromRgb(133, 153, 0),
                    StringColor = Color.FromRgb(42, 161, 152),
                    CommentColor = Color.FromRgb(88, 110, 117),
                    OperatorColor = Color.FromRgb(203, 75, 22),
                    NumberColor = Color.FromRgb(220, 50, 47),
                    BorderColor = Color.FromRgb(7, 54, 66),
                    PrimaryText = Color.FromRgb(253, 246, 227),
                    SecondaryText = Color.FromRgb(147, 161, 161),
                    HoverForeground = Color.FromRgb(253, 246, 227),
                    ActiveForeground = Color.FromRgb(253, 246, 227)
                };

                // Initialize MonokaiPro
                _themes["MonokaiPro"] = new Theme
                {
                    Name = "Monokai Pro",
                    Description = "A refined Monokai theme",
                    IsDark = true,
                    PrimaryBackground = Color.FromRgb(45, 42, 46),
                    SecondaryBackground = Color.FromRgb(52, 49, 53),
                    TertiaryBackground = Color.FromRgb(57, 54, 58),
                    InputBackground = Color.FromRgb(47, 44, 48),
                    HoverBackground = Color.FromRgb(66, 63, 67),
                    SelectionBackground = Color.FromRgb(73, 70, 74),
                    ActiveBackground = Color.FromRgb(62, 59, 63),
                    AccentColor = Color.FromRgb(255, 97, 136),
                    ForegroundColor = Color.FromRgb(252, 252, 252),
                    MenuBackground = Color.FromRgb(45, 42, 46),
                    MenuForeground = Color.FromRgb(252, 252, 252),
                    MenuHighlight = Color.FromRgb(52, 49, 53),
                    ButtonBackground = Color.FromRgb(52, 49, 53),
                    ButtonForeground = Color.FromRgb(252, 252, 252),
                    ButtonHover = Color.FromRgb(66, 63, 67),
                    ButtonPressed = Color.FromRgb(73, 70, 74),
                    SelectionForeground = Color.FromRgb(252, 252, 252),
                    FocusBorder = Color.FromRgb(255, 97, 136),
                    ErrorColor = Color.FromRgb(255, 97, 136),
                    WarningColor = Color.FromRgb(255, 216, 102),
                    SuccessColor = Color.FromRgb(162, 217, 91),
                    LineNumberColor = Color.FromRgb(109, 109, 109),
                    CurrentLineBackground = Color.FromRgb(52, 49, 53),
                    IndentationGuidesColor = Color.FromRgb(73, 70, 74),
                    EditorBackground = Color.FromRgb(45, 42, 46),
                    EditorText = Color.FromRgb(252, 252, 252),
                    EditorLineNumbers = Color.FromRgb(109, 109, 109),
                    EditorCurrentLine = Color.FromRgb(52, 49, 53),
                    EditorSelection = Color.FromRgb(73, 70, 74),
                    KeywordColor = Color.FromRgb(255, 97, 136),
                    StringColor = Color.FromRgb(255, 216, 102),
                    CommentColor = Color.FromRgb(121, 121, 121),
                    OperatorColor = Color.FromRgb(255, 97, 136),
                    NumberColor = Color.FromRgb(174, 129, 255),
                    BorderColor = Color.FromRgb(52, 49, 53),
                    PrimaryText = Color.FromRgb(252, 252, 252),
                    SecondaryText = Color.FromRgb(200, 200, 200),
                    PopupBackground = Color.FromRgb(45, 42, 46),
                    HoverForeground = Color.FromRgb(252, 252, 252),
                    ActiveForeground = Color.FromRgb(252, 252, 252)
                };

                // Set default theme
                CurrentTheme = _themes["OceanDepths"];
                UpdateApplicationTheme();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing themes: {ex.Message}", 
                               "Theme Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void ApplyTheme(string themeName)
        {
            try
            {
                if (_themes.TryGetValue(themeName, out var theme))
                {
                    CurrentTheme = theme;
                }
                else
                {
                    // Default to CozyDark if theme not found
                    CurrentTheme = _themes["CozyDark"];
                }
                UpdateApplicationTheme();
            }
            catch (Exception)
            {
                // If anything goes wrong, fall back to CozyDark
                CurrentTheme = _themes["CozyDark"];
                UpdateApplicationTheme();
            }
        }

        public void ApplyTheme(Theme theme)
        {
            CurrentTheme = theme;
            _themes[theme.Name] = theme;
            UpdateApplicationTheme();
        }

        public void UpdateApplicationTheme()
        {
            try 
            {
                var resources = Application.Current.Resources;
                
                // Colors (for bindings)
                resources["HighlightColor"] = CurrentTheme.AccentColor;
                resources["ForegroundColor"] = CurrentTheme.ForegroundColor;
                
                // Brushes
                resources["BackgroundBrush"] = CreateBrush(CurrentTheme.PrimaryBackground);
                resources["ForegroundBrush"] = CreateBrush(CurrentTheme.ForegroundColor);
                resources["BorderBrush"] = CreateBrush(CurrentTheme.SecondaryBackground);
                resources["HighlightBrush"] = CreateBrush(CurrentTheme.AccentColor);
                
                // Menu specific
                resources["MenuBackgroundBrush"] = CreateBrush(CurrentTheme.MenuBackground);
                resources["MenuForegroundBrush"] = CreateBrush(CurrentTheme.MenuForeground);
                resources["MenuItemForegroundBrush"] = CreateBrush(CurrentTheme.MenuForeground);
                resources["MenuHighlightBrush"] = CreateBrush(CurrentTheme.MenuHighlight);
                
                // Button states
                resources["ButtonBackgroundBrush"] = CreateBrush(CurrentTheme.ButtonBackground);
                resources["ButtonForegroundBrush"] = CreateBrush(CurrentTheme.ButtonForeground);
                resources["ButtonHoverBrush"] = CreateBrush(CurrentTheme.ButtonHover);
                resources["ButtonPressedBrush"] = CreateBrush(CurrentTheme.ButtonPressed);
                
                // Selection and focus
                resources["SelectionBackgroundBrush"] = CreateBrush(CurrentTheme.SelectionBackground);
                resources["SelectionForegroundBrush"] = CreateBrush(CurrentTheme.SelectionForeground);
                resources["FocusBorderBrush"] = CreateBrush(CurrentTheme.FocusBorder);
                
                // Error and warning states
                resources["ErrorBrush"] = CreateBrush(CurrentTheme.ErrorColor);
                resources["WarningBrush"] = CreateBrush(CurrentTheme.WarningColor);
                resources["SuccessBrush"] = CreateBrush(CurrentTheme.SuccessColor);
                
                // Editor specific
                resources["LineNumberBrush"] = CreateBrush(CurrentTheme.LineNumberColor);
                resources["CurrentLineBrush"] = CreateBrush(CurrentTheme.CurrentLineBackground);
                resources["IndentationGuidesBrush"] = CreateBrush(CurrentTheme.IndentationGuidesColor);

                // New brushes
                resources["SecondaryBackgroundBrush"] = new SolidColorBrush(CurrentTheme.SecondaryBackground);
                resources["TertiaryBackgroundBrush"] = new SolidColorBrush(CurrentTheme.TertiaryBackground);
                resources["InputBackgroundBrush"] = new SolidColorBrush(CurrentTheme.InputBackground);
                resources["HoverBackgroundBrush"] = new SolidColorBrush(CurrentTheme.HoverBackground);
                resources["SelectionBackgroundBrush"] = new SolidColorBrush(CurrentTheme.SelectionBackground);
                resources["ActiveBackgroundBrush"] = new SolidColorBrush(CurrentTheme.ActiveBackground);
                resources["PopupBackgroundBrush"] = new SolidColorBrush(CurrentTheme.PopupBackground);

                // Add these new brushes
                resources["HoverForegroundBrush"] = CreateBrush(CurrentTheme.HoverForeground);
                resources["ActiveForegroundBrush"] = CreateBrush(CurrentTheme.ActiveForeground);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating theme: {ex.Message}", "Theme Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private SolidColorBrush CreateBrush(Color color)
        {
            try
            {
                var brush = new SolidColorBrush(color);
                brush.Freeze();
                return brush;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating brush with color: {color}\nError: {ex.Message}",
                               "Brush Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new SolidColorBrush(Colors.Gray);
            }
        }

        public void SaveCustomTheme(Theme theme)
        {
            Directory.CreateDirectory(ThemesDirectory);
            var filePath = Path.Combine(ThemesDirectory, $"{theme.Name}.json");
            var json = JsonSerializer.Serialize(theme);
            File.WriteAllText(filePath, json);
            _themes[theme.Name] = theme;
        }

        private void LoadCustomThemes()
        {
            if (!Directory.Exists(ThemesDirectory)) return;

            foreach (var file in Directory.GetFiles(ThemesDirectory, "*.json"))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var theme = JsonSerializer.Deserialize<Theme>(json);
                    if (theme != null)
                        _themes[theme.Name] = theme;
                }
                catch (Exception ex)
                {
                    // Log theme loading error
                }
            }
        }

        public IEnumerable<Theme> GetCustomThemes()
        {
            return _themes.Values.Where(t => 
                t.Name != "CozyDark" && t.Name != "CheeseCave");
        }

        public void DeleteTheme(string themeName)
        {
            if (_themes.ContainsKey(themeName) && 
                themeName != "CozyDark" && 
                themeName != "CheeseCave")
            {
                _themes.Remove(themeName);
                var filePath = Path.Combine(ThemesDirectory, $"{themeName}.json");
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
} 