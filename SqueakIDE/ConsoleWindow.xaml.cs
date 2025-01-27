using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using SqueakIDE.Windows;

namespace SqueakIDE;
public partial class ConsoleWindow : ModernWindow
{
    private TextWriter _originalOut;
    private TextReader _originalIn;
    private WpfConsoleWriter _writer;
    private WpfConsoleReader _reader;

    public ConsoleWindow()
    {
        InitializeComponent();
        Loaded += Window_Loaded;
        Closing += Window_Closing;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _originalOut = Console.Out;
        _originalIn = Console.In;

        _writer = new WpfConsoleWriter(OutputTextBox);
        _reader = new WpfConsoleReader();

        Console.SetOut(_writer);
        Console.SetIn(_reader);
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Console.SetOut(_originalOut);
        Console.SetIn(_originalIn);
    }

    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string input = InputTextBox.Text;
            InputTextBox.Clear();
            Console.WriteLine($"> {input}");
            _reader.EnqueueLine(input);
            e.Handled = true;
        }
    }

    public void Clear()
    {
        Dispatcher.Invoke(() => OutputTextBox.Clear());
    }
}

public class WpfConsoleWriter : TextWriter
{
    private readonly TextBox _output;
    private readonly Dispatcher _dispatcher;
    private readonly StringBuilder _buffer;
    private readonly System.Timers.Timer _updateTimer;
    private readonly object _lockObject = new object();

    public WpfConsoleWriter(TextBox output)
    {
        _output = output;
        _dispatcher = output.Dispatcher;
        _buffer = new StringBuilder();
        
        _updateTimer = new System.Timers.Timer(100); // Update every 100ms
        _updateTimer.Elapsed += UpdateConsole;
        _updateTimer.Start();
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void WriteLine(string value)
    {
        lock (_lockObject)
        {
            _buffer.AppendLine(value);
        }
    }

    public override void Write(char value)
    {
        lock (_lockObject)
        {
            _buffer.Append(value);
        }
    }

    private void UpdateConsole(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (_buffer.Length == 0) return;

        string text;
        lock (_lockObject)
        {
            text = _buffer.ToString();
            _buffer.Clear();
        }

        _dispatcher.BeginInvoke(new Action(() =>
        {
            _output.AppendText(text);
            _output.ScrollToEnd();
        }), System.Windows.Threading.DispatcherPriority.Background);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _updateTimer.Stop();
            _updateTimer.Dispose();
        }
        base.Dispose(disposing);
    }
}

public class WpfConsoleReader : TextReader
{
    private readonly BlockingCollection<string> _lines = new BlockingCollection<string>();

    public override string ReadLine()
    {
        return _lines.Take();
    }

    public void EnqueueLine(string line)
    {
        _lines.Add(line);
    }
}

public static class ControlExtensions
{
    public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
    {
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);

        if (parentObject == null) return null;

        T parent = parentObject as T;
        if (parent != null)
        {
            return parent;
        }
        else
        {
            return TryFindParent<T>(parentObject);
        }
    }
} 