
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;


public static class ExceptionGenerator
{
    public static string ExceptionTestMessage { get; set; } = "parser test message";

    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    private static Exception GenerateException(Action act)
    {
        try
        {
            act();
            throw new NotImplementedException();
        }
        catch (Exception ex) 
        {
            return ex;
        }
    }

    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static void LoopThenException(int count)
    {
        if (count > 0)
        {
            LoopThenException(--count);
        }
        else
        {
            throw new InvalidOperationException(ExceptionTestMessage);
        }
    }

    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static void GenerateInnerException(int count)
    {
        if (count > 0)
        {
            GenerateInnerException(--count);
        }
        else
        {
            throw new InvalidOperationException(ExceptionTestMessage, GenerateException(() => LoopThenException(5)));
        }
    }
}
