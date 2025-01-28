using System;
using System.Collections.Generic;

namespace SqueakIDE.Debugging;
public class BreakpointManager
{
    public event EventHandler<int> BreakpointChanged;
    
    private readonly Dictionary<int, Breakpoint> _breakpoints = new Dictionary<int, Breakpoint>();
    private readonly IDebuggerService _debugService;

    private void RaiseBreakpointChanged(int line)
    {
        BreakpointChanged?.Invoke(this, line);
    }

    public void ToggleBreakpoint(int line)
    {
        if (_breakpoints.ContainsKey(line))
        {
            RemoveBreakpoint(line);
        }
        else
        {
            AddBreakpoint(line);
        }
    }

    public void AddBreakpoint(int line, string condition = null)
    {
        var breakpoint = new Breakpoint
        {
            Line = line,
            Condition = condition,
            IsEnabled = true
        };

        _breakpoints[line] = breakpoint;
        _debugService.SetBreakpoint(breakpoint);
        
        // Update UI
        RaiseBreakpointChanged(line);
    }

    public void RemoveBreakpoint(int line)
    {
        if (_breakpoints.TryGetValue(line, out var breakpoint))
        {
            _breakpoints.Remove(line);
            _debugService.RemoveBreakpoint(breakpoint);
            
            // Update UI
            RaiseBreakpointChanged(line);
        }
    }
} 