using System;

public class ChatMessage
{
    public string Sender { get; set; }
    public string Content { get; set; }
    public bool IsUser { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
} 