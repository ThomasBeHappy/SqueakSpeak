using ICSharpCode.AvalonEdit;
using SqueakIDE;
using SqueakIDE.Dialogs;
using SqueakIDE.Project;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock;

public class LiveShareClient
{
    private ClientWebSocket _socket;
    private MainWindow _mainWindow;
    private bool _isConnected;
    private Dictionary<string, RemoteCursorAdorner> _remoteCursors = new();
    private TextEditor _editor;
    private AdornerLayer _adornerLayer;
    private string _username;

    public event EventHandler<LiveShareMessage> MessageReceived;
    public bool IsConnected => _isConnected;

    public LiveShareClient(MainWindow mainWindow)
    {
        _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        InitializeEditor();
    }

    private void InitializeEditor()
    {
        _editor = _mainWindow.GetCurrentEditor();
        if (_editor != null)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(_editor.TextArea.TextView);
        }
    }

    public async Task ConnectAsync(string uri)
    {
        try
        {
            var dialog = new UserNameDialog();
            if (dialog.ShowDialog() != true)
            {
                throw new Exception("Username is required for LiveShare.");
            }
            
            _username = dialog.Username;
            _socket = new ClientWebSocket();
            await _socket.ConnectAsync(new Uri(uri), CancellationToken.None);
            _isConnected = true;

            // Start receiving messages
            _ = Task.Run(ReceiveMessagesAsync);
        }
        catch (Exception ex)
        {
            _isConnected = false;
            throw new Exception($"Failed to connect: {ex.Message}");
        }
    }

    public async Task SendFileChangeAsync(string filePath, string content)
    {
        var message = new LiveShareMessage
        {
            Type = "fileChange",
            FilePath = filePath,
            Content = content
        };

        await SendMessageAsync(JsonSerializer.Serialize(message));
    }

    public async Task SendMessageAsync(string message)
    {
        if (_socket?.State != WebSocketState.Open)
        {
            return;
        }

        var buffer = Encoding.UTF8.GetBytes(message);
        await _socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024 * 4];
        var messageBuilder = new StringBuilder();

        while (_socket.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result;
                do
                {
                    result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var messageChunk = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    messageBuilder.Append(messageChunk);
                }
                while (!result.EndOfMessage);

                var message = messageBuilder.ToString();
                messageBuilder.Clear();

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    Console.WriteLine($"Received message: {message}");
                    var data = JsonSerializer.Deserialize<LiveShareMessage>(message);
                    
                    // Handle cursor position
                    if (data.Type == "cursorPosition")
                    {
                        HandleCursorUpdate(data.Username, data);
                    }
                    else
                    {
                        MessageReceived?.Invoke(this, data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving message: {ex}");
                _isConnected = false;
                break;
            }
        }
    }

    private async Task SendCurrentFiles()
    {
        // First send project structure
        if (_mainWindow.CurrentProject != null)
        {
            await SendProjectStructure(_mainWindow.CurrentProject);
        }

        // Then send file contents
        var documents = _mainWindow.GetOpenDocuments();
        foreach (var doc in documents)
        {
            await SendFileChangeAsync(doc.Title, doc.Content);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_socket?.State == WebSocketState.Open)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", CancellationToken.None);
            _isConnected = false;
        }
    }

    public async Task SendProjectStructure(SqueakProject project)
    {
        var message = new LiveShareMessage
        {
            Type = "projectStructure",
            Project = new ProjectStructure
            {
                Name = project.Name,
                RootPath = project.RootPath,
                SourceFiles = project.SourceFiles,
                References = project.References
            }
        };

        await SendMessageAsync(JsonSerializer.Serialize(message));
    }

    internal async void SendCursorPosition(int caretOffset, TextEditor editor, string filePath)
    {
        if (editor == null) return;
        _editor = editor;
        _adornerLayer = AdornerLayer.GetAdornerLayer(_editor.TextArea.TextView);

        try 
        {
            var message = new LiveShareMessage
            {
                Type = "cursorPosition",
                Username = _username,
                FilePath = filePath,
                Content = JsonSerializer.Serialize(new {
                    Offset = caretOffset,
                    SelectionStart = editor.SelectionStart,
                    SelectionLength = editor.SelectionLength
                })
            };

            Console.WriteLine($"Sending cursor position: {JsonSerializer.Serialize(message)}");
            await SendMessageAsync(JsonSerializer.Serialize(message));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending cursor position: {ex}");
        }
    }

    private Color GetRandomColor()
    {
        var random = new Random();
        var hue = random.NextDouble();
        var saturation = 0.5 + random.NextDouble() * 0.5; // 0.5-1.0
        var value = 0.8 + random.NextDouble() * 0.2; // 0.8-1.0

        // Convert HSV to RGB
        var rgb = HsvToRgb(hue, saturation, value);
        return Color.FromRgb(rgb.r, rgb.g, rgb.b);
    }

    private (byte r, byte g, byte b) HsvToRgb(double h, double s, double v)
    {
        var hi = (int)(h * 6) % 6;
        var f = h * 6 - Math.Floor(h * 6);
        var p = v * (1 - s);
        var q = v * (1 - f * s);
        var t = v * (1 - (1 - f) * s);

        var (r, g, b) = hi switch
        {
            0 => (v, t, p),
            1 => (q, v, p),
            2 => (p, v, t),
            3 => (p, q, v),
            4 => (t, p, v),
            _ => (v, p, q)
        };

        return ((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }

    private void HandleCursorUpdate(string username, LiveShareMessage data)
    {
        _mainWindow.Dispatcher.Invoke((Delegate)(() =>
        {
            // Only show cursor if we're in the same file
            _mainWindow.TryGetCurrentEditor(out _editor, out var doc);
            if (doc == null || doc.Title != data.FilePath) return;

            _adornerLayer = AdornerLayer.GetAdornerLayer(_editor.TextArea.TextView);

            if (_editor == null || _adornerLayer == null)
            {
                InitializeEditor();
                if (_editor == null || _adornerLayer == null) return;
            }

            if (!_remoteCursors.TryGetValue(username, out var adorner))
            {
                var color = GetRandomColor();
                adorner = new RemoteCursorAdorner(_editor, username, color);
                _adornerLayer.Add(adorner);
                _remoteCursors[username] = adorner;
            }

            try
            {
                var cursorData = JsonSerializer.Deserialize<CursorData>(data.Content);
                _adornerLayer.Remove(adorner);
                adorner = new RemoteCursorAdorner(_editor, username, adorner.cursorColor.Color);
                _adornerLayer.Add(adorner);
                adorner.UpdatePosition(cursorData.Offset, cursorData.SelectionStart, cursorData.SelectionLength);
                _remoteCursors[username] = adorner;
            }
            catch
            {
                _adornerLayer.Remove(adorner);
                adorner = new RemoteCursorAdorner(_editor, username, adorner.cursorColor.Color);
                _adornerLayer.Add(adorner);
                adorner.UpdatePosition(int.Parse(data.Content));
                _remoteCursors[username] = adorner;
            }

            _adornerLayer.Update();
        }));
    }
}

public class LiveShareMessage
{
    public string Type { get; set; } // "fileChange", "requestFiles", "projectStructure", "cursorPosition"
    public string? FilePath { get; set; }
    public string? Content { get; set; }
    public ProjectStructure? Project { get; set; }
    public string? Username { get; set; }
}

public class ProjectStructure
{
    public string Name { get; set; }
    public string RootPath { get; set; }
    public List<string> SourceFiles { get; set; } = new();
    public List<string> References { get; set; } = new();
}

public class CursorData
{
    public int Offset { get; set; }
    public int SelectionStart { get; set; }
    public int SelectionLength { get; set; }
}
