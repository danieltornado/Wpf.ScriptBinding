using System;
using System.IO;
using ScriptBinding.Internals.Common;
using ScriptBinding.Internals.Executor.ErrorListeners;

namespace ScriptBinding.Debugger.ErrorListeners
{
    class ExecutorErrorListener : IExecutingErrorListener
    {
        private readonly string _expression;
        private readonly TextWriter _writer;

        public ExecutorErrorListener(string expression, TextWriter writer)
        {
            _expression = expression;
            _writer = writer;
        }

        #region Implementation of IExecutingErrorListener

        /// <inheritdoc />
        void IExecutingErrorListener.Error(int start, int end, string message, Exception baseException)
        {
            const string pointer = "^";
            const char empty = '_';

            _writer.WriteLine(_expression);
            _writer.WriteLine(pointer.PadLeft(start, empty) + pointer.PadLeft(end - start, empty));
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
