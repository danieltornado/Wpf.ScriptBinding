using System;
using System.Linq;
using ScriptBinding.Internals.Executor.ErrorListeners;

namespace ScriptBinding.Tests.Internals.Executor.Tools
{
    sealed class ExecutorExceptionListener : IExecutingErrorListener
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

        #region Implementation of IExecuteErrorListener

        /// <inheritdoc />
        public void Error(int start, int end, string message, Exception baseException)
        {
            AddException(baseException);
        }

        #endregion
    }
}
