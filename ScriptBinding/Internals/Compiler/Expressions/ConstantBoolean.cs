using System;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class ConstantBoolean : Expr
    {
        public bool Value { get; }

        /// <inheritdoc />
        public ConstantBoolean(int start, int end, bool value)
            : base(start, end)
        {
            Value = value;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return typeof(bool);
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitConstantBoolean(this);
        }

        #endregion
    }
}
