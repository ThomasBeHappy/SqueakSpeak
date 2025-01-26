using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Input;
using System.Collections.Generic;

namespace SqueakIDE.Utilities
{
    public class AutoClosingBrackets
    {
        private readonly TextArea _textArea;
        private readonly Dictionary<char, char> _bracketPairs = new()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '"', '"' },
            { '\'', '\'' }
        };

        public AutoClosingBrackets(TextArea textArea)
        {
            _textArea = textArea;
            _textArea.TextEntering += TextArea_TextEntering;
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (_bracketPairs.TryGetValue(e.Text[0], out char closingChar))
            {
                var caretOffset = _textArea.Caret.Offset;
                var document = _textArea.Document;
                
                document.Insert(caretOffset, closingChar.ToString());
                _textArea.Caret.Offset = caretOffset;
            }
        }
    }
} 