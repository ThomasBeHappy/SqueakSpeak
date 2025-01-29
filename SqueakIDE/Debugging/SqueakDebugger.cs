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
    private TaskCompletionSource<bool> _continuationSource;
    private DebugStepMode _currentStepMode = DebugStepMode.None;
    private int _stepOutStackDepth = 0;

    public enum DebugStepMode
    {
        None,
        StepOver,
        StepInto,
        StepOut
    }

    public SqueakDebugger(IDebuggerService debugService, DebugVisualizer visualizer)
    {
        _debugService = debugService;
        _visualizer = visualizer;
        
        // Set up debug event handlers
        _debugService.BreakpointHit += OnBreakpointHit;
        _debugService.ExceptionThrown += OnExceptionThrown;
    }

    public async Task StartDebugging()
    {
        _isDebugging = true;
        _continuationSource = new TaskCompletionSource<bool>();
        await _debugService.Initialize();
        _visualizer.ShowDebugOverlay();
    }

    private async void OnBreakpointHit(object sender, BreakpointEventArgs e)
    {
        if (_isDebugging)
        {
            _visualizer.HighlightCurrentLine(e.LineNumber);
            _visualizer.UpdateVariables(e.LocalVariables);
            _visualizer.UpdateCallStack(e.CallStack);

            // Create new continuation point
            _continuationSource = new TaskCompletionSource<bool>();
            
            // Wait for continue signal
            await _continuationSource.Task;
        }
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

    // Add these methods to control execution
    public async void StepOver()
    {
        if (_isDebugging)
        {
            await _debugService.StepOver();
            _continuationSource?.TrySetResult(true);
        }
    }

    public async void StepInto()
    {
        if (_isDebugging)
        {
            await _debugService.StepInto();
            _continuationSource?.TrySetResult(true);
        }
    }

    public async void StepOut()
    {
        if (_isDebugging)
        {
            await _debugService.StepOut();
            _continuationSource?.TrySetResult(true);
        }
    }

    public async void Continue()
    {
        if (_isDebugging)
        {
            await _debugService.Continue();
            _continuationSource?.TrySetResult(true);
        }
    }

    public async void Stop()
    {
        if (_isDebugging)
        {
            _isDebugging = false;
            await _debugService.Stop();
            _continuationSource?.TrySetResult(true);
            _visualizer.HideDebugOverlay();
        }
    }
}