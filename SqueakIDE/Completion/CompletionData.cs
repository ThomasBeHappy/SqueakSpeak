using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace SqueakIDE.Completion
{
    public class CompletionData : ICompletionData
    {
        private readonly CompletionType _type;
        private static readonly Dictionary<CompletionType, ImageSource> _icons = new();

        public CompletionData(string text, string description, CompletionType type)
        {
            Text = text;
            Description = description;
            _type = type;
            
            // Initialize icons if not already done
            if (!_icons.Any())
            {
                _icons[CompletionType.Keyword] = LoadImage("keyword.png");
                _icons[CompletionType.Variable] = LoadImage("variable.png");
                _icons[CompletionType.Function] = LoadImage("function.png");
                _icons[CompletionType.Snippet] = LoadImage("snippet.png");
            }
        }

        private static ImageSource LoadImage(string name)
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = $"SqueakIDE.Resources.{name}";
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) return null;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze(); // Make it thread-safe

                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        public ImageSource Image => _icons.GetValueOrDefault(_type);
        public string Text { get; }
        public object Content => Text;
        public object Description { get; }
        public double Priority => GetPriority(_type);

        private double GetPriority(CompletionType type) => type switch
        {
            CompletionType.Variable => 3,
            CompletionType.Function => 2,
            CompletionType.Keyword => 1,
            CompletionType.Snippet => 0,
            _ => 0
        };

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            if (_type == CompletionType.Snippet)
            {
                // For snippets, try to maintain indentation
                var line = textArea.Document.GetLineByOffset(completionSegment.Offset);
                var lineText = textArea.Document.GetText(line.Offset, line.Length);
                var indentation = lineText.TakeWhile(c => char.IsWhiteSpace(c)).ToArray();
                var indentedText = Text.Replace("\n", "\n" + new string(indentation));
                textArea.Document.Replace(completionSegment, indentedText);
            }
            else
            {
                textArea.Document.Replace(completionSegment, Text);
            }
        }
    }

    public enum CompletionType
    {
        Keyword,
        Variable,
        Function,
        Snippet
    }
} 