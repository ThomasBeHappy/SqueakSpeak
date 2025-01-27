using System.Windows.Media;

namespace SqueakIDE.Themes
{
    public class Theme
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDark { get; set; }

        // Main colors
        public Color PrimaryBackground { get; set; }
        public Color SecondaryBackground { get; set; }
        public Color AccentColor { get; set; }
        public Color ForegroundColor { get; set; }

        // Menu colors
        public Color MenuBackground { get; set; }
        public Color MenuForeground { get; set; }
        public Color MenuHighlight { get; set; }

        // Button colors
        public Color ButtonBackground { get; set; }
        public Color ButtonForeground { get; set; }
        public Color ButtonHover { get; set; }
        public Color ButtonPressed { get; set; }

        // Selection and focus
        public Color SelectionBackground { get; set; }
        public Color SelectionForeground { get; set; }
        public Color FocusBorder { get; set; }

        // Status colors
        public Color ErrorColor { get; set; }
        public Color WarningColor { get; set; }
        public Color SuccessColor { get; set; }

        // Editor specific
        public Color LineNumberColor { get; set; }
        public Color CurrentLineBackground { get; set; }
        public Color IndentationGuidesColor { get; set; }

        // Text colors
        public Color PrimaryText { get; set; }
        public Color SecondaryText { get; set; }

        // Editor colors
        public Color EditorBackground { get; set; }
        public Color EditorText { get; set; }
        public Color EditorLineNumbers { get; set; }
        public Color EditorCurrentLine { get; set; }
        public Color EditorSelection { get; set; }

        // Syntax highlighting colors
        public Color KeywordColor { get; set; }
        public Color StringColor { get; set; }
        public Color CommentColor { get; set; }
        public Color OperatorColor { get; set; }
        public Color NumberColor { get; set; }

        // UI element colors
        public Color BorderColor { get; set; }

        // Additional Background Colors
        public Color TertiaryBackground { get; set; }   // For popups, dropdowns
        public Color InputBackground { get; set; }      // For textboxes, comboboxes
        public Color HoverBackground { get; set; }      // For hover states
        public Color ActiveBackground { get; set; }     // For active/focused elements
        public Color PopupBackground { get; set; }      // For context menus, tooltips

        // Add these new properties
        public Color HoverForeground { get; set; }
        public Color ActiveForeground { get; set; }
    }
} 