using System;

namespace ScriptBinding.Internals.Executor.ErrorListeners
{
    interface IExecutingErrorListener
    {
        void Error(int start, int end, string message, Exception baseException);
    }
}
