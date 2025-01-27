using System;
using System.Collections.Generic;

public static class CommandRegistry
{
    private static readonly Dictionary<string, Action<object[]>> _commands = new();

    public static void Register(string name, Action<object[]> action)
    {
        _commands[name] = action;
    }

    public static bool TryExecute(string name, object[] args)
    {
        if (_commands.TryGetValue(name, out var action))
        {
            action(args);
            return true;
        }
        return false;
    }
} 