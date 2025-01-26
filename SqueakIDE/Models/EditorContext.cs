using System.Collections.Generic;

public class EditorContext
{
    public string CurrentFile { get; set; }
    public string CurrentContent { get; set; }
    public string SelectedText { get; set; }
    public int SelectionStart { get; set; }
    public int SelectionLength { get; set; }
    public Dictionary<string, string> OpenFiles { get; set; } = new();
    public List<string> ProjectFiles { get; set; } = new();
} 