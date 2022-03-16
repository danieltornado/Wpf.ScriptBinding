using System;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    abstract class Expr
    {
        public int Start { get; }
        public int End { get; }

        protected Expr(int start, int end)
        {
            Start = start;
            End = end;
        }

        public abstract Type GetExpressionType();
        public abstract T Accept<T>(IExprVisitor<T> visitor);
    }
}
