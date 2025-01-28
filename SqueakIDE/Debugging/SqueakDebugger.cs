using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;

namespace SqueakIDE.Debugging;
public class SqueakDebugger
{
    private readonly DebugVisualizer _visualizer;
    private readonly IDebuggerService _debugService;
    private bool _isDebugging;
    private readonly Dictionary<int, Breakpoint> _breakpoints = new();

    public SqueakDebugger(IDebuggerService debugService, Grid overlay, Border highlight, ListView variables, ListView callStack)
    {
        _debugService = debugService;
        _visualizer = new DebugVisualizer(overlay, highlight, variables, callStack);
        
        // Set up debug event handlers
        _debugService.BreakpointHit += OnBreakpointHit;
        _debugService.ExceptionThrown += OnExceptionThrown;
    }

    public async Task StartDebugging()
    {
        _isDebugging = true;
        await _debugService.Initialize();
        _visualizer.ShowDebugOverlay();
    }

    private void OnBreakpointHit(object sender, BreakpointEventArgs e)
    {
        _visualizer.HighlightCurrentLine(e.LineNumber);
        _visualizer.UpdateVariables(e.LocalVariables);
        _visualizer.UpdateCallStack(e.CallStack);
    }

    public void OnExceptionThrown(object sender, ExceptionEventArgs ex)
    {
        _debugService.ReportException(ex.Exception);
    }

    public async Task ToggleBreakpoint(Breakpoint breakpoint)
    {
        if (_breakpoints.ContainsKey(breakpoint.Line))
        {
            await _debugService.RemoveBreakpoint(breakpoint);
            _breakpoints.Remove(breakpoint.Line);
        }
        else
        {
            await _debugService.SetBreakpoint(breakpoint);
            _breakpoints.Add(breakpoint.Line, breakpoint);
        }
    }
}