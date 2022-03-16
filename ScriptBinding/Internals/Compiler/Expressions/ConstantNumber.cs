using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    class ConstantNumber : Expr
    {
        [NotNull]
        public object Value { get; }

        /// <inheritdoc />
        public ConstantNumber(int start, int end, [NotNull] object value)
            : base(start, end)
        {
            Value = value;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return Value.GetType();
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitConstantNumber(this);
        }

        #endregion
    }
}
