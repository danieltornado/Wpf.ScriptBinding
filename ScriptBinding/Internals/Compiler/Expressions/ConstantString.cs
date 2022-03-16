using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class ConstantString : Expr
    {
        [NotNull]
        public string Value { get; }

        /// <inheritdoc />
        public ConstantString(int start, int end, [NotNull] string value)
            : base(start, end)
        {
            Value = value;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return typeof(string);
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitConstantString(this);
        }

        #endregion
    }
}
