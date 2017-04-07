
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;


public static class ExceptionGenerator
{
    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static Exception GenerateException(Action act)
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
    public static void LoopThenException(int count, string error_message)
    {
        if (count > 0)
        {
            LoopThenException(--count, error_message);
        }
        else
        {
            throw new InvalidOperationException(error_message);
        }
    }

    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static void GenerateInnerException(int count, string error_message)
    {
        if (count > 0)
        {
            GenerateInnerException(--count, error_message);
        }
        else
        {
            throw new InvalidOperationException(error_message, GenerateException(() => LoopThenException(5, error_message)));
        }
    }
}
