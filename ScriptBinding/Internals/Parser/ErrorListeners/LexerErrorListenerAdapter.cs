using Antlr4.Runtime;

namespace ScriptBinding.Internals.Parser.ErrorListeners
{
    sealed class LexerErrorListenerAdapter : IAntlrErrorListener<int>
    {
        private readonly IScriptErrorListener _listener;

        public LexerErrorListenerAdapter(IScriptErrorListener listener)
        {
            _listener = listener;
        }

        #region Implementation of IAntlrErrorListener<in int>

        /// <inheritdoc />
        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            _listener.SyntaxError(charPositionInLine, msg, e);
        }

        #endregion
    }
}