using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Squeak;

namespace SqueakIDE.Debugging;
public interface IDebuggerService
{
    event EventHandler<BreakpointEventArgs> BreakpointHit;
    event EventHandler<ExceptionEventArgs> ExceptionThrown;
    
    Task Initialize();
    Task SetBreakpoint(Breakpoint breakpoint);
    Task RemoveBreakpoint(Breakpoint breakpoint);
    Task StepOver();
    Task StepInto();
    Task StepOut();
    Task Continue();
    Task Stop();
    void CheckBreakpoint(int line, IEnumerable<DebugVariable> variables, SqueakStackFrame[] callStack);
    void ReportException(Exception ex);
}

public class BreakpointEventArgs : EventArgs
{
    public int LineNumber { get; set; }
    public IEnumerable<DebugVariable> LocalVariables { get; set; }
    public SqueakStackFrame[] CallStack { get; set; }
}

public class ExceptionEventArgs : EventArgs
{
    public Exception Exception { get; set; }
} 