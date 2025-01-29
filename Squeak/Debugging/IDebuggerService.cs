using System;
using System.Collections.Generic;

namespace Squeak;
public interface IDebuggerService
{
    void CheckBreakpoint(int line, IEnumerable<DebugVariable> variables, SqueakStackFrame[] callStack);
    void ReportException(Exception ex);
}

public class DebugVariable
{
    public string Name { get; set; }
    public object Value { get; set; }
    public Type Type { get; set; }
} 