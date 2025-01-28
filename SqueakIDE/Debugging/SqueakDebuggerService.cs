using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SqueakIDE.Debugging;
public class SqueakDebuggerService : IDebuggerService, Squeak.IDebuggerService
{
    private bool _isDebugging = false;
    private bool _isPaused = false;
    
    public event EventHandler<BreakpointEventArgs> BreakpointHit;
    public event EventHandler<ExceptionEventArgs> ExceptionThrown;
    
    private readonly SqueakSpeakInterpreterVisitor _interpreter;
    private readonly Dictionary<int, Breakpoint> _breakpoints = new Dictionary<int, Breakpoint>();

    public SqueakDebuggerService(SqueakSpeakInterpreterVisitor interpreter)
    {
        _interpreter = interpreter;
    }

    public Task Initialize()
    {
        _isDebugging = false;
        _isPaused = false;
        return Task.CompletedTask;
    }

    public Task SetBreakpoint(Breakpoint breakpoint)
    {
        _breakpoints[breakpoint.Line] = breakpoint;
        return Task.CompletedTask;
    }

    public Task RemoveBreakpoint(Breakpoint breakpoint)
    {
        _breakpoints.Remove(breakpoint.Line);
        return Task.CompletedTask;
    }

    public Task StepOver()
    {
        if (_isPaused)
        {
            _isPaused = false;
            // Execute next line only
            Console.WriteLine("Stepping over...");
        }
        return Task.CompletedTask;
    }

    public Task StepInto()
    {
        if (_isPaused)
        {
            _isPaused = false;
            // Step into function if present
            Console.WriteLine("Stepping into...");
        }
        return Task.CompletedTask;
    }

    public Task StepOut()
    {
        if (_isPaused)
        {
            _isPaused = false;
            // Execute until current function returns
            Console.WriteLine("Stepping out...");
        }
        return Task.CompletedTask;
    }

    public Task Continue()
    {
        if (_isPaused)
        {
            _isPaused = false;
            // Continue execution
            Console.WriteLine("Continuing execution...");
        }
        return Task.CompletedTask;
    }

    public Task Stop()
    {
        _isDebugging = false;
        _isPaused = false;
        Console.WriteLine("Debugging stopped");
        return Task.CompletedTask;
    }

    // Interpreter debugger interface implementation
    public void CheckBreakpoint(int line, IEnumerable<DebugVariable> variables, StackFrame[] callStack)
    {
        if (!_isDebugging) return;

        if (_breakpoints.TryGetValue(line, out var breakpoint) && breakpoint.IsEnabled)
        {
            _isPaused = true;
            BreakpointHit?.Invoke(this, new BreakpointEventArgs
            {
                LineNumber = line,
                LocalVariables = variables,
                CallStack = callStack
            });

            while (_isPaused && _isDebugging)
            {
                Task.Delay(100).Wait();
            }
        }
    }

    public void ReportException(Exception ex)
    {
        ExceptionThrown?.Invoke(this, new ExceptionEventArgs { Exception = ex });
    }

    public void CheckBreakpoint(int line, IEnumerable<Squeak.DebugVariable> variables, StackFrame[] callStack)
    {
        CheckBreakpoint(line, variables.Select(v => new DebugVariable { Name = v.Name, Value = v.Value }), callStack);
    }
} 