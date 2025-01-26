using Microsoft.Extensions.Logging;
using System;
using System.Windows.Controls;

public class TextBoxLoggerProvider : ILoggerProvider
{
    private readonly TextBox _output;

    public TextBoxLoggerProvider(TextBox output)
    {
        _output = output;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TextBoxLogger(_output);
    }

    public void Dispose() { }

    private class TextBoxLogger : ILogger
    {
        private readonly TextBox _output;

        public TextBoxLogger(TextBox output)
        {
            _output = output;
        }

        public IDisposable BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                _output.Dispatcher.BeginInvoke(() => 
                {
                    var message = $"[{DateTime.Now:HH:mm:ss}] [{logLevel}] {formatter(state, exception)}";
                    _output.AppendText(message + Environment.NewLine);
                    _output.ScrollToEnd();
                });
            }
        }
    }
} 