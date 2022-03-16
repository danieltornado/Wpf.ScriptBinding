using ScriptBinding.Internals.Compiler;
using ScriptBinding.Internals.Compiler.Expressions;
using ScriptBinding.Tests.Internals.Parser.Tools;

namespace ScriptBinding.Tests.Internals.Compiler.Tools
{
    static class CompilerExtensions
    {
        public static Expr Compile(this string expression)
        {
            var bindingGenerator = new BindingGeneratorMock();
            return expression.Compile(bindingGenerator);
        }

        public static Expr Compile(this string expression, IBindingGenerator bindingGenerator)
        {
            var node = expression.Parse();

            var compiler = new ScriptBinding.Internals.Compiler.Compiler(bindingGenerator);
            var expr = compiler.Compile(node);

            return expr;
        }
    }
}
