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
    private TaskCompletionSource<bool> _pauseCompletionSource;
    private DebugStepMode _currentStepMode = DebugStepMode.None;
    private int _currentStackDepth = 0;
    private StackFrame[] _currentCallStack;
    
    private enum DebugStepMode
    {
        None,
        StepOver,
        StepInto,
        StepOut
    }
    
    public event EventHandler<BreakpointEventArgs> BreakpointHit;
    public event EventHandler<ExceptionEventArgs> ExceptionThrown;
    
    private readonly SqueakSpeakInterpreterVisitor _interpreter;
    private readonly Dictionary<int, Breakpoint> _breakpoints = new();

    public SqueakDebuggerService(SqueakSpeakInterpreterVisitor interpreter)
    {
        _interpreter = interpreter;
    }

    public Task Initialize()
    {
        _isDebugging = true;
        _isPaused = false;
        _currentStepMode = DebugStepMode.None;
        _currentStackDepth = 0;
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
            _currentStepMode = DebugStepMode.StepOver;
            _currentStackDepth = _currentCallStack?.Length ?? 0;
            Resume();
        }
        return Task.CompletedTask;
    }

    public Task StepInto()
    {
        if (_isPaused)
        {
            _currentStepMode = DebugStepMode.StepInto;
            Resume();
        }
        return Task.CompletedTask;
    }

    public Task StepOut()
    {
        if (_isPaused)
        {
            _currentStepMode = DebugStepMode.StepOut;
            _currentStackDepth = _currentCallStack?.Length ?? 0;
            Resume();
        }
        return Task.CompletedTask;
    }

    public Task Continue()
    {
        if (_isPaused)
        {
            _currentStepMode = DebugStepMode.None;
            Resume();
        }
        return Task.CompletedTask;
    }

    private void Resume()
    {
        _isPaused = false;
        _pauseCompletionSource?.TrySetResult(true);
    }

    public Task Stop()
    {
        _isDebugging = false;
        _isPaused = false;
        _currentStepMode = DebugStepMode.None;
        _pauseCompletionSource?.TrySetResult(true);
        return Task.CompletedTask;
    }

    // Interpreter debugger interface implementation
    public void CheckBreakpoint(int line, IEnumerable<DebugVariable> variables, StackFrame[] callStack)
    {
        _currentCallStack = callStack;  // Store the call stack
        if (!_isDebugging) return;

        bool shouldBreak = false;
        var currentDepth = callStack?.Length ?? 0;  // Declare once at the top
        
        // Check if we should break based on step mode
        switch (_currentStepMode)
        {
            case DebugStepMode.None:
                shouldBreak = _breakpoints.TryGetValue(line, out var breakpoint);
                break;
                
            case DebugStepMode.StepInto:
                shouldBreak = true;
                break;
                
            case DebugStepMode.StepOver:
                shouldBreak = currentDepth <= _currentStackDepth;
                break;
                
            case DebugStepMode.StepOut:
                shouldBreak = currentDepth < _currentStackDepth;
                break;
        }

        if (shouldBreak)
        {
            _isPaused = true;
            _pauseCompletionSource = new TaskCompletionSource<bool>();
            
            // Notify debugger about the break
            BreakpointHit?.Invoke(this, new BreakpointEventArgs
            {
                LineNumber = line,
                LocalVariables = variables,
                CallStack = callStack
            });

            // Wait for resume signal
            _pauseCompletionSource.Task.Wait();
            
            // Reset step mode if it was a single-step operation
            if (_currentStepMode != DebugStepMode.None)
            {
                _currentStepMode = DebugStepMode.None;
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