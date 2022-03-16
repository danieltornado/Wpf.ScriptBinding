using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser.Tools
{
    static class ParserExtensions
    {
        public static Node Parse(this string expression)
        {
            var errorListener = new ParserExceptionListener();
            var parser = new ScriptBinding.Internals.Parser.Parser(errorListener);
            var result = parser.Parse(expression);

            errorListener.RaiseExceptionIfExist();

            return result;
        }
    }
}
