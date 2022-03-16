using System;
using System.Diagnostics;
using ScriptBinding.Internals.Common;

namespace ScriptBinding.Internals.Executor.ErrorListeners
{
    sealed class DebugExecutingErrorListener : IExecutingErrorListener
    {
        #region Implementation of IExecutingErrorListener

        /// <inheritdoc />
        public void Error(int start, int end, string message, Exception baseException)
        {
            if (baseException != null)
            {
                Debug.WriteLine($"System.Windows.Data Error: executing at position {start}-{end}: {message}; reason: {baseException.GetFullMessage()}");
            }
            else
            {
                Debug.WriteLine($"System.Windows.Data Error: executing at position {start}-{end}: {message}");
            }
        }

        #endregion
    }
}
