using System;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallBinding : Expr
    {
        public int Index { get; }

        /// <inheritdoc />
        public CallBinding(int start, int end, int index)
            : base(start, end)
        {
            Index = index;
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
            return visitor.VisitCallBinding(this);
        }

        #endregion
    }
}
