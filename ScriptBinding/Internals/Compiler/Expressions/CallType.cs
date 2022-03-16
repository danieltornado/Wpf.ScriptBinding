using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallType : Expr
    {
        [NotNull]
        public Type Type { get; }

        /// <inheritdoc />
        public CallType(int start, int end, [NotNull] Type type)
            : base(start, end)
        {
            Type = type;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return Type;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitCallType(this);
        }

        #endregion
    }
}
