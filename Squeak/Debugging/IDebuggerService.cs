using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Squeak;
public interface IDebuggerService
{
    void CheckBreakpoint(int line, IEnumerable<DebugVariable> variables, StackFrame[] callStack);
    void ReportException(Exception ex);
}

public class DebugVariable
{
    public string Name { get; set; }
    public object Value { get; set; }
    public Type Type { get; set; }
} 