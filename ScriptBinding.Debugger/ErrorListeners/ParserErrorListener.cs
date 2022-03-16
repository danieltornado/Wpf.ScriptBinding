using System;
using System.IO;
using ScriptBinding.Internals.Common;
using ScriptBinding.Internals.Parser.ErrorListeners;

namespace ScriptBinding.Debugger.ErrorListeners
{
    class ParserErrorListener : IScriptErrorListener
    {
        private readonly string _expression;
        private readonly TextWriter _writer;

        public ParserErrorListener(string expression, TextWriter writer)
        {
            _expression = expression;
            _writer = writer;
        }

        #region Implementation of IScriptErrorListener

        /// <inheritdoc />
        void IScriptErrorListener.SyntaxError(int position, string message, Exception baseException)
        {
            const string pointer = "^";
            const char empty = '_';

            _writer.WriteLine(_expression);
            _writer.WriteLine(pointer.PadLeft(position, empty));
            _writer.WriteLine(message);

            if (baseException != null)
            {
                _writer.WriteLine("baseException: " + baseException.GetFullMessage());
            }

            _writer.WriteLine();
        }

        #endregion
    }
}
