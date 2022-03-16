using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallMethod : Expr
    {
        [NotNull]
        public Expr Target { get; }

        [NotNull]
        public MethodInfo Method { get; }

        [NotNull]
        public IReadOnlyList<Expr> Parameters { get; }

        /// <inheritdoc />
        public CallMethod(int start, int end, [NotNull] Expr target, [NotNull] MethodInfo method, [NotNull] IReadOnlyList<Expr> parameters)
            : base(start, end)
        {
            Target = target;
            Method = method;
            Parameters = parameters;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return Method.ReturnType;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitCallMethod(this);
        }

        #endregion
    }
}
