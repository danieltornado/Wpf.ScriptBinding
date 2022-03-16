using System;

namespace ScriptBinding.Internals.Parser.ErrorListeners
{
    interface IScriptErrorListener
    {
        void SyntaxError(int position, string message, Exception baseException);
    }
}
