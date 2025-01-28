using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SqueakIDE.Debugging;

public class DebugCommands
{
    private readonly SqueakDebugger _debugger;
    private readonly IDebuggerService _debugService;

    public ICommand StartDebugging { get; }
    public ICommand StepOver { get; }
    public ICommand StepInto { get; }
    public ICommand StepOut { get; }
    public ICommand Continue { get; }
    public ICommand Stop { get; }

    public DebugCommands(SqueakDebugger debugger, IDebuggerService debugService)
    {
        _debugger = debugger;
        _debugService = debugService;

        StartDebugging = new AsyncRelayCommand(ExecuteStartDebugging);
        StepOver = new AsyncRelayCommand(ExecuteStepOver);
        StepInto = new AsyncRelayCommand(ExecuteStepInto);
        StepOut = new AsyncRelayCommand(ExecuteStepOut);
        Continue = new AsyncRelayCommand(ExecuteContinue);
        Stop = new AsyncRelayCommand(ExecuteStop);
    }

    private async Task ExecuteStartDebugging()
    {
        await _debugger.StartDebugging();
    }

    private async Task ExecuteStepOver()
    {
        await _debugService.StepOver();
    }

    private async Task ExecuteStepInto()
    {
        await _debugService.StepInto();
    }

    private async Task ExecuteStepOut()
    {
        await _debugService.StepOut();
    }

    private async Task ExecuteContinue()
    {
        await _debugService.Continue();
    }

    private async Task ExecuteStop()
    {
        await _debugService.Stop();
    }
} 