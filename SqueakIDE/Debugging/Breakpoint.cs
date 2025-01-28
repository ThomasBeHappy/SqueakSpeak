namespace SqueakIDE.Debugging;
public class Breakpoint
{
    public int Line { get; set; }
    public bool IsEnabled { get; set; }
    public string Condition { get; set; }
} 