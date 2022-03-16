using Antlr4.Runtime;

namespace ScriptBinding.Internals.Parser.ErrorListeners
{
    sealed class ParserErrorListenerAdapter : IAntlrErrorListener<IToken>
    {
        private readonly IScriptErrorListener _listener;

        public ParserErrorListenerAdapter(IScriptErrorListener listener)
        {
            _listener = listener;
        }

        #region Implementation of IAntlrErrorListener<in IToken>

        /// <inheritdoc />
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            _listener.SyntaxError(charPositionInLine, msg, e);
        }

        #endregion
    }
}
