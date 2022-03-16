using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class Parens : Expr
    {
        [NotNull]
        public Expr Expression { get; }

        /// <inheritdoc />
        public Parens(int start, int end, [NotNull] Expr expression)
            : base(start, end)
        {
            Expression = expression;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return Expression.GetExpressionType();
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitParens(this);
        }

        #endregion
    }
}
