using System;
using System.Linq;
using ScriptBinding.Internals.Parser.ErrorListeners;

namespace ScriptBinding.Tests.Internals.Parser.Tools
{
    sealed class ParserExceptionListener : IScriptErrorListener
    {
        private Exception _exception;

        public void RaiseExceptionIfExist()
        {
            if (_exception != null)
                throw _exception;
        }

        private void AddException(Exception exception)
        {
            if (_exception == null)
            {
                _exception = exception;
            }
            else if (_exception is AggregateException e)
            {
                _exception = new AggregateException(e.InnerExceptions.Concat(Enumerable.Repeat(exception, 1)));
            }
            else
            {
                _exception = new AggregateException(Enumerable.Repeat(_exception, 1).Concat(Enumerable.Repeat(exception, 1)));
            }
        }

        #region Implementation of IScriptErrorListener

        /// <inheritdoc />
        public void SyntaxError(int position, string message, Exception baseException)
        {
            AddException(new ParsingScriptException(position, message, baseException));
        }

        #endregion
    }
}
