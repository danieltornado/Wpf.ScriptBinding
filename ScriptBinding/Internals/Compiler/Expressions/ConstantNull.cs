using System;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class ConstantNull : Expr
    {
        /// <inheritdoc />
        public ConstantNull(int start, int end)
            : base(start, end)
        {
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
            return visitor.VisitConstantNull(this);
        }

        #endregion
    }
}
