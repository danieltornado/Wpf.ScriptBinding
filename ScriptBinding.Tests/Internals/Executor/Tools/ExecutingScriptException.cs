using System;

namespace ScriptBinding.Tests.Internals.Executor.Tools
{
    sealed class ExecutingScriptException : Exception
    {
        internal ExecutingScriptException(int start, int end, string message, Exception innerException)
            : base(message, innerException)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}
