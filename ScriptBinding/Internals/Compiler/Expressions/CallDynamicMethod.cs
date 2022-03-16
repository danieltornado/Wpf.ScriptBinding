using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallDynamicMethod : Expr
    {
        [NotNull]
        public Expr Target { get; }

        [NotNull]
        public string MethodName { get; }

        [NotNull]
        public IReadOnlyList<Expr> Parameters { get; }

        /// <inheritdoc />
        public CallDynamicMethod(int start, int end, [NotNull] Expr target, [NotNull] string methodName, [NotNull] IReadOnlyList<Expr> parameters)
            : base(start, end)
        {
            Target = target;
            MethodName = methodName;
            Parameters = parameters;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return null;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitCallDynamicMethod(this);
        }

        #endregion
    }
}
