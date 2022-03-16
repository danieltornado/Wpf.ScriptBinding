using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class Conditional : Expr
    {
        [NotNull]
        public Expr If { get; }

        [NotNull]
        public Expr Then { get; }

        [NotNull]
        public Expr Else { get; }

        /// <inheritdoc />
        public Conditional(int start, int end, [NotNull] Expr ifExpr, [NotNull] Expr thenExpr, [NotNull] Expr elseExpr)
            : base(start, end)
        {
            If = ifExpr;
            Then = thenExpr;
            Else = elseExpr;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            Type firstType = Then.GetExpressionType();
            if (firstType != null)
            {
                Type secondType = Else.GetExpressionType();
                if (firstType == secondType)
                    return firstType;
            }

            return null;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitConditional(this);
        }

        #endregion
    }
}
