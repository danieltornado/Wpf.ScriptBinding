using System;

namespace ScriptBinding.Tests.Internals.Parser.Tools
{
    public sealed class ParsingScriptException : Exception
    {
        internal ParsingScriptException(int position, string message, Exception innerException)
            : base(message, innerException)
        {
            Position = position;
        }

        public int Position { get; }
    }
}
