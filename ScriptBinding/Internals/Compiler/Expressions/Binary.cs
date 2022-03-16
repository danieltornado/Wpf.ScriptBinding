using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class Binary : Expr
    {
        [NotNull]
        public Expr Argument1 { get; }

        [NotNull]
        public Expr Argument2 { get; }

        public BinaryType OperationType { get; }

        /// <inheritdoc />
        public Binary(int start, int end, [NotNull] Expr argument1, [NotNull] Expr argument2, BinaryType operationType)
            : base(start, end)
        {
            Argument1 = argument1;
            Argument2 = argument2;
            OperationType = operationType;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            // TODO: implement
            return null;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitBinary(this);
        }

        #endregion
    }
}
