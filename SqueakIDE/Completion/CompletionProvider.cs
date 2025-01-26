using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using System;

namespace SqueakIDE.Completion
{
    public class CompletionProvider
    {
        private readonly List<CompletionData> _keywords;
        private CompletionWindow _completionWindow;
        private HashSet<string> _variables = new HashSet<string>();
        private Dictionary<string, HashSet<string>> _objectFields = new Dictionary<string, HashSet<string>>();

        public CompletionProvider()
        {
            // Initialize with SqueakSpeak keywords from grammar
            _keywords = new List<CompletionData>
            {
                // Core Keywords
                new CompletionData("Squeak", "Output a value to the console", CompletionType.Keyword),
                new CompletionData("Cuddle", "Declare a variable", CompletionType.Keyword),
                new CompletionData("Nuzzle", "While loop", CompletionType.Keyword),
                new CompletionData("FluffMagic", "Define a function", CompletionType.Keyword),
                new CompletionData("Peek", "If statement", CompletionType.Keyword),
                new CompletionData("Purr", "Else statement", CompletionType.Keyword),
                new CompletionData("SnipChoose", "Switch statement", CompletionType.Keyword),
                new CompletionData("SnipCase", "Case in switch statement", CompletionType.Keyword),
                new CompletionData("SnipDefault", "Default case in switch statement", CompletionType.Keyword),
                new CompletionData("BringWarmth", "Import another file", CompletionType.Keyword),
                new CompletionData("PawReturn", "Return statement", CompletionType.Keyword),
                
                // Extended Features
                new CompletionData("BeepBoop", "Make HTTP requests", CompletionType.Keyword),
                new CompletionData("Listen", "Get user input", CompletionType.Keyword),
                new CompletionData("Brain", "Perform math operations", CompletionType.Keyword),
                new CompletionData("SnuggleObject", "Create an object", CompletionType.Keyword),
                new CompletionData("NativeCall", "Call native C# methods", CompletionType.Keyword),
                new CompletionData("NativeFunc", "Call native C# functions", CompletionType.Keyword),

                // Common Math Functions (for Brain)
                new CompletionData("sin", "Sine function", CompletionType.Function),
                new CompletionData("cos", "Cosine function", CompletionType.Function),
                new CompletionData("tan", "Tangent function", CompletionType.Function),
                new CompletionData("sqrt", "Square root function", CompletionType.Function),
                new CompletionData("pow", "Power function", CompletionType.Function),
                
                // Snippets
                new CompletionData("FluffMagic main() {\n    \n}", "Main function", CompletionType.Snippet),
                new CompletionData("Peek (x == y) {\n    \n}", "If statement", CompletionType.Snippet),
                new CompletionData("Nuzzle (condition) {\n    \n}", "While loop", CompletionType.Snippet),
                new CompletionData("SnuggleObject myObj {\n    field->value = 0;\n}", "Object creation", CompletionType.Snippet),
                new CompletionData("Brain(sin, x) -> result;", "Math operation", CompletionType.Snippet)
            };
        }
        
        public void CloseCompletion()
        {
            if (_completionWindow != null)
            {
                _completionWindow.Close();
                _completionWindow = null;
            }
        }

        public void ShowCompletion(TextArea textArea)
        {
            // Close any existing completion window to ensure fresh context
            CloseCompletion();

            // Get the word and context before caret
            var (wordBeforeCaret, isObjectAccess, objectName, wordStartOffset) = GetCompletionContext(textArea);
            
            // Scan for variables and objects in the current document
            ScanForDeclarations(textArea.Document.Text);
            
            // Create new completion window with the correct segment
            _completionWindow = new CompletionWindow(textArea);
            _completionWindow.StartOffset = wordStartOffset;
            _completionWindow.Closed += (s, args) => _completionWindow = null;
            StyleCompletionWindow(_completionWindow);

            var data = _completionWindow.CompletionList.CompletionData;
            
            if (isObjectAccess && _objectFields.ContainsKey(objectName))
            {
                System.Diagnostics.Debug.WriteLine($"Showing fields for {objectName}");
                // Show object fields
                var fields = _objectFields[objectName]
                    .Where(f => f.StartsWith(wordBeforeCaret, StringComparison.OrdinalIgnoreCase))
                    .Select(f => new CompletionData(f, $"Field of {objectName}", CompletionType.Variable));
                foreach (var field in fields)
                {
                    data.Add(field);
                }
            }
            else
            {
                // Show regular completions
                var matchingKeywords = _keywords.Where(item => 
                    item.Text.StartsWith(wordBeforeCaret, StringComparison.OrdinalIgnoreCase));
                
                var matchingVariables = _variables
                    .Where(v => v.StartsWith(wordBeforeCaret, StringComparison.OrdinalIgnoreCase))
                    .Select(v => new CompletionData(v, "Variable", CompletionType.Variable));

                foreach (var item in matchingKeywords.Concat(matchingVariables))
                {
                    data.Add(item);
                }
            }

            if (data.Count > 0)
            {
                _completionWindow.Show();
                // Pre-select the first item
                _completionWindow.CompletionList.SelectedItem = data.FirstOrDefault();
                _completionWindow.CompletionList.ListBox.SelectedIndex = 0;
            }
            else
            {
                CloseCompletion();
            }
        }

        private (string word, bool isObjectAccess, string objectName, int wordStartOffset) GetCompletionContext(TextArea textArea)
        {
            var caretPosition = textArea.Caret.Offset;
            var document = textArea.Document;
            var line = document.GetLineByOffset(caretPosition);
            var lineText = document.GetText(line.Offset, line.Length);
            var columnInLine = caretPosition - line.Offset;

            // Check if we're after an arrow operator
            var arrowIndex = lineText.LastIndexOf("->", columnInLine);
            if (arrowIndex >= 0 && arrowIndex < columnInLine)
            {
                // Find the object name before the arrow
                var objectStart = arrowIndex;
                while (objectStart > 0 && (char.IsLetterOrDigit(lineText[objectStart - 1]) || lineText[objectStart - 1] == '_'))
                {
                    objectStart--;
                }
                var objectName = lineText.Substring(objectStart, arrowIndex - objectStart).Trim();

                // Get the partial field name after the arrow
                var fieldStart = arrowIndex + 2;
                var fieldLength = columnInLine - fieldStart;
                var partialField = fieldLength > 0 ? lineText.Substring(fieldStart, fieldLength).Trim() : "";

                System.Diagnostics.Debug.WriteLine($"Line text: '{lineText}'");
                System.Diagnostics.Debug.WriteLine($"Arrow at: {arrowIndex}, Column: {columnInLine}");
                System.Diagnostics.Debug.WriteLine($"Object: '{objectName}', Partial: '{partialField}'");

                if (!string.IsNullOrWhiteSpace(objectName))
                {
                    return (partialField, true, objectName, line.Offset + fieldStart);
                }
            }

            // Regular word completion
            var wordStart = caretPosition;
            while (wordStart > 0)
            {
                var charAt = document.GetCharAt(wordStart - 1);
                if (!char.IsLetterOrDigit(charAt) && charAt != '_') break;
                wordStart--;
            }

            var word = caretPosition > wordStart 
                ? document.GetText(wordStart, caretPosition - wordStart) 
                : string.Empty;

            return (word, false, string.Empty, wordStart);
        }

        private void ScanForDeclarations(string text)
        {
            _variables.Clear();
            _objectFields.Clear();
            
            // Scan for Cuddle declarations and function parameters (existing code)
            ScanForVariables(text);

            // Scan for SnuggleObject declarations
            var objectPattern = @"SnuggleObject\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*\{([^}]*)\}";
            var fieldPattern = @"([a-zA-Z_][a-zA-Z0-9_]*)->([a-zA-Z_][a-zA-Z0-9_]*)\s*=";

            var objectMatches = System.Text.RegularExpressions.Regex.Matches(text, objectPattern);
            foreach (System.Text.RegularExpressions.Match objMatch in objectMatches)
            {
                var objectName = objMatch.Groups[1].Value;
                var objectBody = objMatch.Groups[2].Value;
                
                var fields = new HashSet<string>();
                var fieldMatches = System.Text.RegularExpressions.Regex.Matches(objectBody, fieldPattern);
                
                foreach (System.Text.RegularExpressions.Match fieldMatch in fieldMatches)
                {
                    fields.Add(fieldMatch.Groups[2].Value);
                }

                _objectFields[objectName] = fields;
                _variables.Add(objectName); // Add the object itself as a variable
            }
        }

        private void StyleCompletionWindow(CompletionWindow window)
        {
            window.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            window.Foreground = Brushes.White;
            
            var list = window.CompletionList;
            list.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            list.Foreground = Brushes.White;
            
            // Style the list box
            if (list.ListBox != null)
            {
                list.ListBox.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
                list.ListBox.Foreground = Brushes.White;
                list.ListBox.BorderBrush = new SolidColorBrush(Color.FromRgb(45, 45, 45));
                
                // Enable selection by typing
                list.ListBox.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                list.ListBox.IsSynchronizedWithCurrentItem = true;
            }
        }

        private void ScanForVariables(string text)
        {
            _variables.Clear();
            
            // Match Cuddle declarations
            var cuddlePattern = @"Cuddle\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*[=;]";
            var cuddleMatches = System.Text.RegularExpressions.Regex.Matches(text, cuddlePattern);
            foreach (System.Text.RegularExpressions.Match match in cuddleMatches)
            {
                _variables.Add(match.Groups[1].Value);
            }

            // Match function parameters
            var funcPattern = @"FluffMagic\s+[a-zA-Z_][a-zA-Z0-9_]*\s*\((.*?)\)";
            var funcMatches = System.Text.RegularExpressions.Regex.Matches(text, funcPattern);
            foreach (System.Text.RegularExpressions.Match match in funcMatches)
            {
                var parameters = match.Groups[1].Value.Split(',')
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrEmpty(p));
                foreach (var param in parameters)
                {
                    _variables.Add(param);
                }
            }
        }
    }
} 