using Antlr4.Runtime;
using SqueakIDE.Models;
using System.Collections.Generic;
using System.IO;


namespace SqueakIDE
{
    // Custom error listener to collect syntax errors
    public class ErrorCollector : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        public List<SyntaxError> Errors { get; } = new();

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(new SyntaxError
            {
                Line = line,
                Column = charPositionInLine,
                StartIndex = offendingSymbol,
                Length = 1,
                Message = msg
            });
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(new SyntaxError
            {
                Line = line,
                Column = charPositionInLine,
                StartIndex = offendingSymbol.StartIndex,
                Length = offendingSymbol.StopIndex - offendingSymbol.StartIndex + 1,
                Message = msg
            });
        }
    }
}
