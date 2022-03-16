using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    class Unary : Expr
    {
        [NotNull]
        public Expr Argument { get; }

        public UnaryType OperationType { get; }

        /// <inheritdoc />
        public Unary(int start, int end, [NotNull] Expr argument, UnaryType operationType)
            : base(start, end)
        {
            Argument = argument;
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
            return visitor.VisitUnary(this);
        }

        #endregion
    }
}
