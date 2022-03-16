using System;
using System.Diagnostics;
using ScriptBinding.Internals.Common;

namespace ScriptBinding.Internals.Parser.ErrorListeners
{
    sealed class DebugParserErrorListener : IScriptErrorListener
    {
        #region Implementation of IScriptErrorListener

        /// <inheritdoc />
        public void SyntaxError(int position, string message, Exception baseException)
        {
            if (baseException != null)
            {
                Debug.WriteLine($"System.Windows.Data Error: parsing at position {position}: {message}; reason: {baseException.GetFullMessage()}");
            }
            else
            {
                Debug.WriteLine($"System.Windows.Data Error: parsing at position {position}: {message}");
            }
        }

        #endregion


    }
}
