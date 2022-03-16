using ScriptBinding.Internals.Executor;
using ScriptBinding.Tests.Internals.Compiler.Tools;

namespace ScriptBinding.Tests.Internals.Executor.Tools
{
    static class ExecutorExtensions
    {
        public static object Execute(this string expression, IBindingProvider bindingProvider)
        {
            var expr = expression.Compile();

            var errorListener = new ExecutorExceptionListener();
            var executor = new ScriptBinding.Internals.Executor.Executor(errorListener, bindingProvider);
            var result = executor.Execute(expr);

            errorListener.RaiseExceptionIfExist();

            return result;
        }
    }
}
