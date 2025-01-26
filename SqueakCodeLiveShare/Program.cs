using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebSockets(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseWebSockets();

var sessions = new ConcurrentDictionary<string, Session>();

app.Map("/liveshare/{sessionId}", async context =>
{
    var sessionId = context.Request.RouteValues["sessionId"]?.ToString();
    Console.WriteLine($"Received connection request for session: {sessionId}");

    if (string.IsNullOrEmpty(sessionId) || !sessions.TryGetValue(sessionId, out var session))
    {
        Console.WriteLine($"Session not found: {sessionId}");
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Session not found");
        return;
    }

    if (context.WebSockets.IsWebSocketRequest)
    {
        Console.WriteLine($"Accepting WebSocket connection for session: {sessionId}");
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var clientId = Guid.NewGuid().ToString();
        session.Clients.TryAdd(clientId, socket);
        Console.WriteLine($"Client {clientId} joined session {sessionId}");

        while (socket.State == WebSocketState.Open)
        {
            var buffer = new byte[1024 * 4];
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message from client {clientId}: {message}");
                var messageData = JsonSerializer.Deserialize<LiveShareMessage>(message);

                if (messageData.Type == "fileChange")
                {
                    Console.WriteLine($"File change received: {messageData.FilePath}");
                    // Update file content in session
                    if (messageData.FilePath != null && messageData.Content != null)
                    {
                        session.FileContents[messageData.FilePath] = messageData.Content;

                        // Broadcast the file change to all clients
                        foreach (var client in session.Clients.Values)
                        {
                            if (client != socket)
                            {
                                Console.WriteLine($"Broadcasting file change to other clients");
                                await client.SendAsync(
                                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
                            }
                        }
                    }
                }
                else if (messageData.Type == "projectStructure")
                {
                    Console.WriteLine($"Project structure received: {messageData.Project?.Name}");
                    session.ProjectStructure = messageData.Project;
                    
                    // Broadcast to other clients
                    foreach (var client in session.Clients.Values)
                    {
                        if (client != socket)
                        {
                            await client.SendAsync(
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                        }
                    }
                }
                else if (messageData.Type == "requestFiles")
                {
                    Console.WriteLine($"Files requested by client {clientId}");
                    
                    // Send project structure first if available
                    if (session.ProjectStructure != null)
                    {
                        var projectMessage = new LiveShareMessage
                        {
                            Type = "projectStructure",
                            Project = session.ProjectStructure
                        };
                        var projectMessageJson = JsonSerializer.Serialize(projectMessage);
                        await socket.SendAsync(
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes(projectMessageJson)),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }

                    // Then send file contents
                    foreach (var file in session.FileContents)
                    {
                        var fileMessage = new LiveShareMessage
                        {
                            Type = "fileChange",
                            FilePath = file.Key,
                            Content = file.Value
                        };
                        var fileMessageJson = JsonSerializer.Serialize(fileMessage);
                        await socket.SendAsync(
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes(fileMessageJson)),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                }
                else if (messageData.Type == "cursorPosition")
                {
                    Console.WriteLine($"Cursor position received from {clientId}: {messageData.Content}");
                    messageData.Username = clientId;
                    var cursorMessage = JsonSerializer.Serialize(messageData);
                    
                    foreach (var client in session.Clients.Values)
                    {
                        if (client != socket)
                        {
                            await client.SendAsync(
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes(cursorMessage)),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                        }
                    }
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine($"Client {clientId} disconnecting from session {sessionId}");
                session.Clients.TryRemove(clientId, out _);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected", CancellationToken.None);
            }
        }
    }
    else
    {
        Console.WriteLine("Received non-WebSocket request");
        context.Response.StatusCode = 400;
    }
});

app.MapPost("/create-session", async context =>
{
    var sessionId = Guid.NewGuid().ToString();
    var session = new Session();
    sessions[sessionId] = session;

    // Change https:// to ws:// for WebSocket connections
    var scheme = context.Request.Scheme == "https" ? "wss" : "ws";
    var link = $"{scheme}://{context.Request.Host}/liveshare/{sessionId}";
    Console.WriteLine($"Created new session: {sessionId} with link: {link}");
    await context.Response.WriteAsJsonAsync(new { link });
});

Console.WriteLine("LiveShare server starting...");
app.Run();
Console.WriteLine("LiveShare server stopped.");

public class LiveShareMessage
{
    public string Type { get; set; } // "fileChange", "requestFiles", "projectStructure"
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

record Session
{
    public ConcurrentDictionary<string, WebSocket> Clients { get; } = new();
    public ConcurrentDictionary<string, string> FileContents { get; } = new();
    public ProjectStructure? ProjectStructure { get; set; }
}
