using Antlr4.Runtime;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqueakIDE.Services;

public class ValidationService
{
    private readonly Dictionary<string, List<string>> _errors = new();
    private readonly Dictionary<string, List<string>> _warnings = new();

    public void ValidateFile(string filePath, string content, MainWindow window)
    {
        _errors.Remove(filePath);
        _warnings.Remove(filePath);

        var fileErrors = new List<string>();
        var fileWarnings = new List<string>();

        // Basic syntax validation
        ValidateSyntax(content, fileErrors, fileWarnings, window);

        if (fileErrors.Any())
            _errors[filePath] = fileErrors;
        if (fileWarnings.Any())
            _warnings[filePath] = fileWarnings;
    }

    private void ValidateSyntax(string content, List<string> errors, List<string> warnings, MainWindow window)
    {

        var parser = CreateParser(content);
        var errorListener = new ErrorCollector();
        parser.RemoveErrorListeners();
        parser.AddErrorListener(errorListener);

        parser.program();
        DisplayValidationResults(parser, errorListener, window);


        // Check for unmatched brackets/parentheses
        var brackets = new Stack<char>();
        var bracketPairs = new Dictionary<char, char> 
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' }
        };

        for (int i = 0; i < content.Length; i++)
        {
            char c = content[i];
            if (bracketPairs.ContainsKey(c))
            {
                brackets.Push(c);
            }
            else if (bracketPairs.ContainsValue(c))
            {
                if (brackets.Count == 0 || bracketPairs[brackets.Pop()] != c)
                {
                    errors.Add($"Mismatched bracket at position {i}");
                }
            }
        }

        if (brackets.Count > 0)
        {
            errors.Add("Unclosed brackets detected");
        }

        // Check for common issues
        if (content.Contains("TODO"))
            warnings.Add("TODO comment found");

        if (content.Contains("Debug."))
            warnings.Add("Debug statement found");
    }

    private void DisplayValidationResults(SqueakSpeakParser parser, ErrorCollector errorListener, MainWindow window)
    {
        window.ErrorList.Items.Clear();

        if (parser.NumberOfSyntaxErrors == 0)
        {
            window.OutputBox.Text = "Code is valid!";
        }
        else
        {
            foreach (var error in errorListener.Errors)
            {
                var errorDetails = $"Syntax Error at line {error.Line}, column {error.Column}: {error.Message}";
                window.ErrorList.Items.Add(errorDetails);
            }
        }
    }


    private SqueakSpeakParser CreateParser(string code)
    {
        var inputStream = new AntlrInputStream(code);
        var lexer = new SqueakSpeakLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        return new SqueakSpeakParser(tokens);
    }

    public IReadOnlyDictionary<string, List<string>> GetErrors() => _errors;
    public IReadOnlyDictionary<string, List<string>> GetWarnings() => _warnings;

    public bool HasErrors(string filePath) => _errors.ContainsKey(filePath);
    public bool HasWarnings(string filePath) => _warnings.ContainsKey(filePath);
} 