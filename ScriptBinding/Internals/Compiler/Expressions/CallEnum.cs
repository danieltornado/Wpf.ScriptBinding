using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallEnum : Expr
    {
        [NotNull]
        public Enum Value { get; }

        /// <inheritdoc />
        public CallEnum(int start, int end, [NotNull] Enum value)
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
            return visitor.VisitCallEnum(this);
        }

        #endregion
    }
}
