using System;

namespace SqueakIDE.Debugging;
public class DebugVariable
{
    public string Name { get; set; }
    public object Value { get; set; }
    public Type Type { get; set; }
} 